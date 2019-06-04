using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Reflection;
using System;
using System.Reflection;

namespace PostSharp.Samples.Authorization.Framework
{
  /// <summary>
  ///   Aspect that, when applied on a field or property, requires a permission when the field or property is being get or
  ///   set.
  /// </summary>
  /// <remarks>
  ///   Note that this aspect is not a custom attribute and cannot be applied directly to a field or a property. It is
  ///   indirectly created by the <see cref="RequiresPermissionAttribute" /> aspect.
  /// </remarks>
  [Serializable]
  [AspectTypeDependency(AspectDependencyAction.Commute, typeof(LocationAuthorizationAspect))]
  public class LocationAuthorizationAspect : AuthorizationAspect, ILocationInterceptionAspect
  {
    internal LocationAuthorizationAspect()
    {
    }

    /// <summary>
    ///   Method executed at run time to initialize the aspect.
    /// </summary>
    /// <param name="locationInfo">Field or property on which the aspect has been applied.</param>
    public void RuntimeInitialize(LocationInfo locationInfo)
    {
      InitializePermissions(1, new[] { OperationSemantic.Read, OperationSemantic.Write });
    }

    /// <summary>
    ///   Method executed when the field or property is being read.
    /// </summary>
    /// <param name="args">Description of the current operation.</param>
    public void OnGetValue(LocationInterceptionArgs args)
    {
      RequirePermission(args.Location.FieldInfo ?? (MemberInfo) args.Location.PropertyInfo, OperationSemantic.Read, 0,
        args.Instance);

      args.ProceedGetValue();
    }

    /// <summary>
    ///   Method executed when the field or property is being read.
    /// </summary>
    /// <param name="args">Description of the current operation.</param>
    public void OnSetValue(LocationInterceptionArgs args)
    {
      RequirePermission(args.Location.FieldInfo ?? (MemberInfo) args.Location.PropertyInfo, OperationSemantic.Write, 0,
        args.Instance);

      args.ProceedSetValue();
    }
  }
}