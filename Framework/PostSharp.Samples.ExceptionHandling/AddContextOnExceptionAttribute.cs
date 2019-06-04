using PostSharp.Aspects;
using PostSharp.Serialization;
using System;
using System.Text;

namespace PostSharp.Samples.ExceptionHandling
{
  /// <summary>
  ///   Aspect that, when applied to a method and whenever this method fails with an exception, adds the value of method
  ///   parameters to the <see cref="Exception" /> object.
  ///   The <see cref="ReportAndSwallowExceptionAttribute" /> can consume this information and print it if the exception is
  ///   not handled.
  /// </summary>
  [PSerializable]
  public sealed class AddContextOnExceptionAttribute : OnExceptionAspect
  {
    /// <summary>
    ///   Method invoked when the target method fails with an exception.
    /// </summary>
    /// <param name="args">Method invocation context.</param>
    public override void OnException(MethodExecutionArgs args)
    {
      // Get or create a StringBuilder for the exception where we will add additional context data.
      var stringBuilder = (StringBuilder) args.Exception.Data["Context"];
      if (stringBuilder == null)
      {
        stringBuilder = new StringBuilder();
        args.Exception.Data["Context"] = stringBuilder;
      }

      // Append context to the StringBuilder.
      AppendCallInformation(args, stringBuilder);
      stringBuilder.AppendLine();
    }

    private static void AppendCallInformation(MethodExecutionArgs args, StringBuilder stringBuilder)
    {
      var declaringType = args.Method.DeclaringType;
      Formatter.AppendTypeName(stringBuilder, declaringType);
      stringBuilder.Append('.');
      stringBuilder.Append(args.Method.Name);

      if (args.Method.IsGenericMethod)
      {
        var genericArguments = args.Method.GetGenericArguments();
        Formatter.AppendGenericArguments(stringBuilder, genericArguments);
      }

      var arguments = args.Arguments;

      Formatter.AppendArguments(stringBuilder, arguments);
    }
  }
}