using System;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends;

namespace PostSharp.Samples.Logging.Loupe
{
  /// <summary>
  /// Logging back-end for Loupe.
  /// </summary>
  [Log(AttributeExclude = true)]
  public class LoupeLoggingBackend : TextLoggingBackend
  {

    /// <inheritdoc />
    public override LogRecordBuilder CreateRecordBuilder()
    {
      return new LoupeLogRecordBuilder(this);
    }

    /// <inheritdoc />
    protected override TextLoggingBackendOptions GetTextBackendOptions()
    {
      return this.Options;
    }

    /// <inheritdoc />
    protected override LoggingTypeSource CreateTypeSource(LoggingNamespaceSource parent, Type type)
    {
      return new LoupeLoggingTypeSource(parent, type);
    }

    /// <summary>
    /// Gets the backend options.
    /// </summary>
    public new LoupeLoggingBackendOptions Options { get; } = new LoupeLoggingBackendOptions();

  }
}
