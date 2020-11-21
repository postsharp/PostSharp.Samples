using System;

namespace PostSharp.Samples.Logging.Audit
{
    /// <summary>
    /// Arguments of the <see cref="AuditServices.RecordPublished"/> event.
    /// </summary>
    public class AuditRecordEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the published record.
        /// </summary>
        public AuditRecord Record { get; }

        /// <summary>
        /// Initializes a new <see cref="AuditRecordEventArgs"/>.
        /// </summary>
        /// <param name="record">The published <see cref="AuditRecord"/>.</param>
        public AuditRecordEventArgs( AuditRecord record )
        {
            this.Record = record;
        }
    }
}
