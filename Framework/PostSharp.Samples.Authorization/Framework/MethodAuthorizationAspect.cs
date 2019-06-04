using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using System;
using System.Reflection;

namespace PostSharp.Samples.Authorization.Framework
{
  /// <summary>
  ///   Aspect that, when applied on a method, requires a permission when the method is being executed,
  /// </summary>
  /// <remarks>
  ///   Note that this aspect is not a custom attribute and cannot be applied directly to a field or a property. It is
  ///   indirectly created by the <see cref="RequiresPermissionAttribute" /> aspect.
  /// </remarks>
  [Serializable]
  [AspectTypeDependency(AspectDependencyAction.Commute, typeof(MethodAuthorizationAspect))]
  public class MethodAuthorizationAspect : AuthorizationAspect, IOnMethodBoundaryAspect
  {
    /// <summary>
    ///   Method executed at run time to initialize the aspect.
    /// </summary>
    /// <param name="method">Method on which the aspect has been applied.</param>
    public void RuntimeInitialize(MethodBase method)
    {
      InitializePermissions(method.GetParameters().Length + 1, new[] { OperationSemantic.Default });
    }

    /// <summary>
    ///   Method executed before the method is being executed.
    /// </summary>
    /// <param name="args">Contextual information about the current operation.</param>
    public void OnEntry(MethodExecutionArgs args)
    {
      if (HasPermissionForParameter(0))
      {
        RequirePermission(args.Method, OperationSemantic.Default, 0, args.Instance);
      }

      for (var i = 0; i < args.Arguments.Count; i++)
      {
        RequirePermission(args.Method, OperationSemantic.Default, i + 1, args.Arguments[i]);
      }
    }

    public void OnExit(MethodExecutionArgs args)
    {
      // Nothing to do.
    }

    public void OnSuccess(MethodExecutionArgs args)
    {
      // Nothing to do.
    }


    public void OnException(MethodExecutionArgs args)
    {
      // Nothing to do.
    }
  }
}