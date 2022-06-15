using PostSharp.Patterns.Diagnostics.Contexts;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using PostSharp.Patterns.Formatters;
using System;

namespace PostSharp.Samples.Logging.Audit.Audit.Backend
{
  /// <summary>
  /// Record builder for the <see cref="AuditBackend"/>.
  /// </summary>
  public class AuditRecordBuilder : TextLogRecordBuilder
  {

    /// <summary>
    /// Creates an <see cref="AuditRecord"/>.
    /// </summary>
    /// <param name="context">The context of the record.</param>
    /// <param name="recordInfo">Information about the record.</param>
    /// <param name="memberInfo">Information about the method, property, or field.</param>
    /// <returns>An <see cref="AuditRecord"/>.</returns>
    protected virtual AuditRecord CreateRecord(LoggingContext context, ref LogRecordInfo recordInfo, ref LogMemberInfo memberInfo)
    {
#pragma warning disable CS0618 // Type or member is obsolete
      return new AuditRecord(memberInfo.Source.SourceType, memberInfo.MemberName, recordInfo.RecordKind);
#pragma warning restore CS0618 // Type or member is obsolete
    }

    /// <inheritdoc />
    public override void BeginRecord(LoggingContext context, ref LogRecordInfo recordInfo, ref LogMemberInfo memberInfo)
    {
      base.BeginRecord(context, ref recordInfo, ref memberInfo);

      this.CurrentRecord = this.CreateRecord(context, ref recordInfo, ref memberInfo);

    }

    /// <summary>
    /// Gets the current <see cref="AuditBackend"/>.
    /// </summary>
    public new AuditBackend Backend
    {
      get;
    }

    /// <summary>
    /// Initializes a new <see cref="AuditRecordBuilder"/>.
    /// </summary>
    /// <param name="backend">The parent <see cref="AuditBackend"/>.</param>
    public AuditRecordBuilder(AuditBackend backend) : base(backend)
    {
      this.Backend = backend;
    }

    /// <summary>
    /// Gets the <see cref="AuditRecord"/> being currently built.
    /// </summary>
    public AuditRecord CurrentRecord
    {
      get; private set;
    }

    /// <inheritdoc />
    public override void SetException(Exception exception)
    {
      base.SetException(exception);

      this.CurrentRecord.Exception = exception;
    }

    /// <inheritdoc />
    public override void SetThis<T>(T value, IFormatter<T> formatter)
    {
      // We don't want to include the value in the text.
      // base.SetThis<T>(value, formatter);

      this.CurrentRecord.Target = value;
    }

    /// <inheritdoc />
    protected override void Write(UnsafeString message)
    {
      if (this.CurrentRecord != null)
      {
        this.CurrentRecord.Text = this.Context.Description.ToString();
        this.Backend.PublishRecord(this.CurrentRecord);
        this.CurrentRecord = null;
      }
    }
  }
}