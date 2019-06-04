using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PostSharp.Samples.Authorization.Framework
{
  /// <summary>
  ///   Base class for <see cref="RequiresPermissionAttribute" />. This abstraction does not make any assumption regarding
  ///   the implementation of the <see cref="IPermission" />, i.e. it does not assume that a permission is simply a named
  ///   object.
  ///   It could be used to implementation for instance parametric permissions.
  /// </summary>
  [Serializable]
  [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = true)]
  public abstract class RequiresPermissionBaseAttribute : Attribute, IPermissionFactory, IAspectProvider
  {
    /// <summary>
    ///   Invoked at build time to get the list of aspects required to implement the current custom attribute.
    /// </summary>
    /// <param name="targetElement">Declaration on which the attribute has been applied.</param>
    /// <returns>A collection of aspect instances.</returns>
    public virtual IEnumerable<AspectInstance> ProvideAspects(object targetElement)
    {
      var aspectRepositoryService =
        PostSharpEnvironment.CurrentProject.GetService<IAspectRepositoryService>();

      var aspects = aspectRepositoryService.GetAspectInstances(targetElement) ?? new IAspectInstance[0];


      var aspect = (AuthorizationAspect) aspects.Select(i => i.Aspect).SingleOrDefault(i => i is AuthorizationAspect);

      if (aspect == null)
      {
        if (targetElement is MethodBase)
        {
          aspect = new MethodAuthorizationAspect();
        }
        else if (targetElement is PropertyInfo || targetElement is FieldInfo)
        {
          aspect = new LocationAuthorizationAspect();
        }
        else if (targetElement is ParameterInfo)
        {
          aspect = new MethodAuthorizationAspect();
        }
        else
        {
          throw new Exception($"Unexpected aspect target: {targetElement.GetType().Name}");
        }

        yield return new AspectInstance(targetElement, aspect);
      }

      var parameterIndex = 0;

      if (targetElement is ParameterInfo parameterInfo)
      {
        parameterIndex = parameterInfo.Position + 1;
      }

      aspect.AddPermission(parameterIndex, this);
    }

    /// <inheritdoc />
    public abstract IPermission CreatePermission(OperationSemantic semantic);
  }
}