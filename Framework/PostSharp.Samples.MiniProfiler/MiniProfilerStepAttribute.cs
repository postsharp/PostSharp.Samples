using PostSharp.Aspects;
using PostSharp.Serialization;
using StackExchange.Profiling;
using System;
using System.Reflection;

namespace PostSharp.Samples.MiniProfiler
{
  [PSerializable]
  [LinesOfCodeAvoided(2)]
  public sealed class MiniProfilerStepAttribute : OnMethodBoundaryAspect
  {
    private string methodName;

    public MiniProfilerStepAttribute()
    {
      SemanticallyAdvisedMethodKinds = SemanticallyAdvisedMethodKinds.Async;
    }

    public override bool CompileTimeValidate(MethodBase method)
    {
      // Don't apply the aspect to constructors, property getters, and so on.
      if (method.IsSpecialName)
      {
        return false;
      }

      return true;
    }

    public override void CompileTimeInitialize(MethodBase method, AspectInfo aspectInfo)
    {
      methodName = method.DeclaringType.Name + "." + method.Name;
    }


    public override void OnEntry(MethodExecutionArgs args)
    {
      args.MethodExecutionTag = StackExchange.Profiling.MiniProfiler.Current?.Step(methodName);
    }

    public override void OnExit(MethodExecutionArgs args)
    {
      ((IDisposable) args.MethodExecutionTag)?.Dispose();
    }
  }
}