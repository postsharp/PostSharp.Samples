using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using PostSharp.Patterns.Formatters;
using PostSharp.Reflection;

namespace PostSharp.Samples.Logging
{
  [Log(AttributeExclude = true)]
  internal class CustomLoggingBackend : ConsoleLoggingBackend
  {
    public override LogRecordBuilder CreateRecordBuilder()
    {
      return new RecordBuilder(this);
    }


    [Log(AttributeExclude = true)]
    private class RecordBuilder : ConsoleLogRecordBuilder
    {
      public RecordBuilder(ConsoleLoggingBackend backend) : base(backend)
      {
      }

      public override void SetParameter<T>(
        int index, string parameterName, ParameterKind parameterKind, string typeName, T value, IFormatter<T> formatter)
      {
        StringBuilder.Append('[');
        StringBuilder.Append(parameterKind.ToString());
        StringBuilder.Append(']', ' ');

        base.SetParameter(index, parameterName, parameterKind, typeName, value, formatter);
      }
    }
  }
}