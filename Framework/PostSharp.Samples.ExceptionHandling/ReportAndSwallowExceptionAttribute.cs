using PostSharp.Aspects;
using PostSharp.Aspects.Dependencies;
using PostSharp.Serialization;
using System;
using System.Text;

namespace PostSharp.Samples.ExceptionHandling
{
  /// <summary>
  ///   Aspect that, when applied to a method, reports and then swallows (ignores) any exception that this method may
  ///   throw.
  ///   The aspect also prints the data collected by the <see cref="AddContextOnExceptionAttribute" /> aspect.
  /// </summary>
  /// <remarks>
  ///   <para>
  ///     Do not indiscriminately apply this aspect to all methods as exceptions are generally useful. Only use apply
  ///     this aspect
  ///     to thread entry points or event handlers.
  ///   </para>
  /// </remarks>
  // Specify that this aspect must orderd after AddContextOnExceptionAttribute.
  [AspectTypeDependency(AspectDependencyAction.Order, AspectDependencyPosition.After,
    typeof(AddContextOnExceptionAttribute))]
  [PSerializable]
  public sealed class ReportAndSwallowExceptionAttribute : OnExceptionAspect
  {
    public override void OnException(MethodExecutionArgs args)
    {
      // Write the default exception information.
      Console.WriteLine("Exception information");
      Console.WriteLine("--------------------------------------------------------------");
      Console.WriteLine(args.Exception.ToString());
      Console.WriteLine("--------------------------------------------------------------");
      var additionalContext = (StringBuilder) args.Exception.Data["Context"];

      // Write the additional information that was gathered by AddContextOnExceptionAttribute.
      if (additionalContext != null)
      {
        Console.WriteLine("Additional context information (call stack with parameter values)");
        Console.WriteLine("--------------------------------------------------------------");
        Console.Write(additionalContext.ToString());
        Console.WriteLine("--------------------------------------------------------------");
      }

      // Ignore the exception.
      Console.WriteLine("*** Ignoring the exception ***");
      args.FlowBehavior = FlowBehavior.Continue;
    }
  }
}