using PostSharp.Patterns.Diagnostics;

namespace PostSharp.Samples.Logging.Audit
{
    /// <summary>
    /// Custom attribute that, when applied to a method, causes PostSharp to audit execution of this method. Whenever an audited method is executed, the 
    /// <see cref="AuditServices.RecordPublished"/> event is raised. You need to register your own logic to the <see cref="AuditServices.RecordPublished"/> event,
    /// for instance to append the record to a database table.
    /// </summary>
    /// <remarks>
    /// <para>Audit is a special kind of logging. The <see cref="AuditAttribute"/> aspects adds logging with the <c>Audit</c> profile, which is by default served
    /// by the <see cref="AuditBackend"/> back-end, which exposes a simple API to the <see cref="AuditServices"/> class.
    /// </para>
    /// <para>The <see cref="AuditAttribute"/> aspect can be used side-by-side with the <see cref="LogAttribute"/> aspect.</para>
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes")]
    public class AuditAttribute : LogAttributeBase
    {
        /// <summary>
        /// Initializes a new <see cref="AuditAttribute"/>.
        /// </summary>
        public AuditAttribute()
        {
            this.ProfileName = "Audit";
        }
    }
}
