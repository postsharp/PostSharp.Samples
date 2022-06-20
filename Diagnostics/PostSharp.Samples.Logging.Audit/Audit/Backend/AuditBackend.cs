using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using System;

namespace PostSharp.Samples.Logging.Audit.Audit.Backend
{
  /// <summary>
  /// A specialized back-end that publishes audit records to the <see cref="AuditServices.RecordPublished"/> event.
  /// </summary>
  public class AuditBackend : TextLoggingBackend
  {
    /// <summary>
    /// Initializes a new <see cref="AuditBackend"/>.
    /// </summary>
    public AuditBackend()
    {
    }


    /// <summary>
    /// Publishes an <see cref="AuditRecord"/> to the <see cref="AuditServices.RecordPublished"/> event.
    /// </summary>
    /// <param name="record"></param>
    public virtual void PublishRecord(AuditRecord record)
    {
      AuditServices.PublishRecord(record);
    }


    /// <inheritdoc />
    protected override LoggingTypeSource CreateTypeSource(LoggingNamespaceSource parent, Type type)
    {
      return new AuditTypeSource(parent, type);
    }

    /// <inheritdoc />
    public override LogRecordBuilder CreateRecordBuilder()
    {
      return new AuditRecordBuilder(this);
    }

    /// <summary>
    /// Gets the back-end options.
    /// </summary>
    public new AuditBackendOptions Options { get; } = new AuditBackendOptions();

    /// <inheritdoc />
    protected override TextLoggingBackendOptions GetTextBackendOptions() => this.Options;
  }
}