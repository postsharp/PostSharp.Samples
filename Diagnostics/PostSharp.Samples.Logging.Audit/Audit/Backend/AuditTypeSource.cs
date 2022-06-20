using PostSharp.Patterns.Diagnostics;
using System;

namespace PostSharp.Samples.Logging.Audit.Audit.Backend
{
  /// <summary>
  /// Represents a type logged using the <see cref="AuditBackend"/>.
  /// </summary>
  public class AuditTypeSource : LoggingTypeSource
  {
    /// <summary>
    /// Initializes a new <see cref="AuditTypeSource"/>.
    /// </summary>
    /// <param name="parent">The parent <see cref="LoggingNamespaceSource"/>.</param>
    /// <param name="type">The declaring type of audited methods.</param>
    public AuditTypeSource(LoggingNamespaceSource parent, Type type) : base(parent, type.FullName, type)
    {
    }

    /// <inheritdoc />
    protected override bool IsBackendEnabled(LogLevel level)
    {
      return true;
    }
  }
}