using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using System;

namespace PostSharp.Samples.Logging.CustomBackend.ServiceStack
{
  public class ServiceStackLoggingBackend : TextLoggingBackend
  {
    public new ServiceStackLoggingBackendOptions Options { get; } = new ServiceStackLoggingBackendOptions();

    protected override LoggingTypeSource CreateTypeSource(LoggingNamespaceSource parent, Type type)
    {
      return new ServiceStackLoggingTypeSource(parent, type);
    }

    public override LogRecordBuilder CreateRecordBuilder()
    {
      return new ServiceStackLogRecordBuilder(this);
    }

    protected override TextLoggingBackendOptions GetTextBackendOptions()
    {
      return Options;
    }
  }
}