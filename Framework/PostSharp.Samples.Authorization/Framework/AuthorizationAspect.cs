using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security;

namespace PostSharp.Samples.Authorization.Framework
{
  /// <summary>
  ///   Base class for <see cref="MethodAuthorizationAspect" /> and <see cref="LocationAuthorizationAspect" />.
  /// </summary>
  [PSerializable]
  public abstract class AuthorizationAspect : IAspect
  {
    [ThreadStatic] private static bool evaluatingPermissions;

    private BitArray _hasPermissionForParameter;

    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private List<OperationPermission<IPermissionFactory>> _permissionFactories =
      new List<OperationPermission<IPermissionFactory>>();

    private OperationPermission<IPermission>[] _permissions;

    /// <summary>
    ///   Adds a permission for a given semantic and index parameter. This method is invoked at build time.
    /// </summary>
    /// <param name="parameterIndex">
    ///   The 1-based index of the parameter on which the permission is required, or 0 if the
    ///   permission is required on the <c>this</c> object.
    /// </param>
    /// <param name="permissionFactory">
    ///   The <see cref="IPermissionFactory" /> that will be used to instantiate the permission
    ///   at run time.
    /// </param>
    internal void AddPermission(int parameterIndex, IPermissionFactory permissionFactory)
    {
      _permissionFactories.Add(
        new OperationPermission<IPermissionFactory>(OperationSemantic.Default, parameterIndex, permissionFactory));
    }

    /// <summary>
    ///   Instantiates all permissions. This method is called at run=time.
    /// </summary>
    /// <param name="parameterCount">The number of parameters in the method, plus 1 for the <c>this</c> parameter.</param>
    /// <param name="semantics">The set of semantics for which permissions are initialized.</param>
    internal void InitializePermissions(int parameterCount, OperationSemantic[] semantics)
    {
      _hasPermissionForParameter = new BitArray(parameterCount);
      _permissions = new OperationPermission<IPermission>[_permissionFactories.Count * semantics.Length];

      var i = 0;
      foreach (var permissionFactory in _permissionFactories)
      {
        foreach (var semantic in semantics)
        {
          _permissions[i] = new OperationPermission<IPermission>(semantic, permissionFactory.ParameterIndex,
            permissionFactory.Permission.CreatePermission(semantic));

          _hasPermissionForParameter[permissionFactory.ParameterIndex] = true;
          i++;
        }
      }
    }

    /// <summary>
    ///   Determines whether there is at least one permission required for the given parameter.
    /// </summary>
    /// <param name="parameterIndex">
    ///   The 1-based index of the parameter on which the permission is required, or 0 if the
    ///   permission is required on the <c>this</c> object.
    /// </param>
    /// <returns></returns>
    internal bool HasPermissionForParameter(int parameterIndex)
    {
      return _hasPermissionForParameter[parameterIndex];
    }

    /// <summary>
    ///   Requires a permission for the given operation semantic, parameter index and securable object.
    /// </summary>
    /// <param name="member">The member, field or property being accessed.</param>
    /// <param name="semantic">The semantic of the operation being executed.</param>
    /// <param name="parameterIndex">
    ///   The 1-based index of the parameter on which the permission is required, or 0 if the
    ///   permission is required on the <c>this</c> object.
    /// </param>
    /// <param name="securable"></param>
    internal void RequirePermission(MemberInfo member, OperationSemantic semantic, int parameterIndex, object securable)
    {
      if (evaluatingPermissions)
      {
        return;
      }

      if (SecurityContext.Current == null)
      {
        return;
      }

      var subject = SecurityContext.Current.Subject;
      var policy = SecurityContext.Current.Policy;

      if (policy == null)
      {
        return;
      }

      try
      {
        evaluatingPermissions = true;

        foreach (var permission in _permissions)
        {
          if ((permission.Semantic == OperationSemantic.Default || permission.Semantic == semantic) &&
              permission.ParameterIndex == parameterIndex)
          {
            if (!policy.Evaluate(subject, permission.Permission, securable))
            {
              SecurityContext.Current.ExceptionHandler?.OnSecurityException(member, semantic, securable,
                SecurityContext.Current.Subject, permission.Permission);

              string memberKind;
              if (member is FieldInfo)
              {
                memberKind = "field";
              }
              else if (member is PropertyInfo)
              {
                memberKind = "property";
              }
              else if (member is MethodBase)
              {
                memberKind = "method";
              }
              else
              {
                throw new ArgumentOutOfRangeException(nameof(member));
              }

              throw new SecurityException(
                $"Cannot {semantic.ToString().ToLowerInvariant()} the {memberKind} {member.DeclaringType.Name}.{member.Name}: the subject '{subject.Name}' does not have the {permission.Permission.Name} permission on the object '{securable}'.");
            }
          }
        }
      }
      finally
      {
        evaluatingPermissions = false;
      }
    }


    [PSerializable]
    private struct OperationPermission<T>
    {
      public OperationPermission(OperationSemantic semantic, int parameterIndex, T permission)
      {
        Semantic = semantic;
        Permission = permission;
        ParameterIndex = parameterIndex;
      }

      public OperationSemantic Semantic { get; private set; }

      public int ParameterIndex { get; private set; }

      public T Permission { get; private set; }
    }
  }
}