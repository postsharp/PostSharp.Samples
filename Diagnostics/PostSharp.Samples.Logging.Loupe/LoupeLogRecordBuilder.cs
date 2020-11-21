using System;
using System.Linq;
using Gibraltar.Agent;
using System.Text;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using PostSharp.Patterns.Formatters;

namespace PostSharp.Samples.Logging.Loupe
{
  /// <summary>
  /// <see cref="LogRecordBuilder"/> for <see cref="LoupeLoggingBackend"/>.
  /// </summary>
  [Log(AttributeExclude = true)]
  public class LoupeLogRecordBuilder : TextLogRecordBuilder, IMessageSourceProvider
  {
    /// <summary>
    /// Initializes a new <see cref="LoupeLogRecordBuilder"/>.
    /// </summary>
    /// <param name="backend">The parent <see cref="LoupeLoggingBackend"/>.</param>
    public LoupeLogRecordBuilder(LoupeLoggingBackend backend) : base(backend)
    {
    }

    string IMessageSourceProvider.MethodName => this.MemberName;
    string IMessageSourceProvider.ClassName => this.TypeSource.FullName;
    string IMessageSourceProvider.FileName => this.SourceLineInfo.File;
    int IMessageSourceProvider.LineNumber => this.SourceLineInfo.Line;

    /// <inheritdoc />
    protected override void AppendSourceLineInfo()
    {
      // We don't append source line info to the text since Gibraltar supports it natively.
    }

    /// <inheritdoc />
    protected override void Write(UnsafeString message)
    {
      Log.Write(TranslateLevel(this.Level), "PostSharp", this, null, this.Exception, LogWriteMode.Queued,
        null, this.TypeSource.Role + "." + this.RecordKind.ToString(), message.ToString(), null);
    }

    private static LogMessageSeverity TranslateLevel(LogLevel level)
    {
      switch (level)
      {
        case LogLevel.None:
          return LogMessageSeverity.None;

        case LogLevel.Trace:
          return LogMessageSeverity.Verbose;

        case LogLevel.Debug:
          return LogMessageSeverity.Verbose;

        case LogLevel.Info:
          return LogMessageSeverity.Information;

        case LogLevel.Warning:
          return LogMessageSeverity.Warning;

        case LogLevel.Error:
          return LogMessageSeverity.Error;

        case LogLevel.Critical:
          return LogMessageSeverity.Critical;

        default:
          return LogMessageSeverity.Information;
      }
    }
  }
}
