using PostSharp.Patterns.Diagnostics.Backends;

namespace PostSharp.Samples.Logging.Audit.Audit.Backend
{
  /// <summary>
  /// Options of <see cref="AuditBackend"/>.
  /// </summary>
  public class AuditBackendOptions : TextLoggingBackendOptions
  {
    /// <summary>
    /// Initializes a new <see cref="AuditBackendOptions"/>.
    /// </summary>
    public AuditBackendOptions()
    {
      this.IndentSpaces = 0;
    }
  }
}