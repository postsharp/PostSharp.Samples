using PostSharp.Aspects;
using PostSharp.Serialization;
using System.Reflection;
using System.Text;

namespace PostSharp.Samples.CustomLogging.Aspects
{
  /// <summary>
  ///   Aspect that, when applied to a method, appends a record to the <see cref="Logger" /> class whenever this method is
  ///   executed.
  /// </summary>
  [PSerializable]
  [LinesOfCodeAvoided(6)]
  public sealed class LogMethodAttribute : OnMethodBoundaryAspect
  {
    /// <summary>
    ///   Method invoked before the target method is executed.
    /// </summary>
    /// <param name="args">Method execution context.</param>
    public override void OnEntry(MethodExecutionArgs args)
    {
      var stringBuilder = new StringBuilder();

      stringBuilder.Append("Entering ");
      AppendCallInformation(args, stringBuilder);
      Logger.WriteLine(stringBuilder.ToString());

      Logger.Indent();
    }


    /// <summary>
    ///   Method invoked after the target method has successfully completed.
    /// </summary>
    /// <param name="args">Method execution context.</param>
    public override void OnSuccess(MethodExecutionArgs args)
    {
      Logger.Unindent();

      var stringBuilder = new StringBuilder();

      stringBuilder.Append("Exiting ");
      AppendCallInformation(args, stringBuilder);

      if (!args.Method.IsConstructor && ((MethodInfo) args.Method).ReturnType != typeof(void))
      {
        stringBuilder.Append(" with return value ");
        stringBuilder.Append(args.ReturnValue);
      }

      Logger.WriteLine(stringBuilder.ToString());
    }

    /// <summary>
    ///   Method invoked when the target method has failed.
    /// </summary>
    /// <param name="args">Method execution context.</param>
    public override void OnException(MethodExecutionArgs args)
    {
      Logger.Unindent();

      var stringBuilder = new StringBuilder();

      stringBuilder.Append("Exiting ");
      AppendCallInformation(args, stringBuilder);

      if (!args.Method.IsConstructor && ((MethodInfo) args.Method).ReturnType != typeof(void))
      {
        stringBuilder.Append(" with exception ");
        stringBuilder.Append(args.Exception.GetType().Name);
      }

      Logger.WriteLine(stringBuilder.ToString());
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