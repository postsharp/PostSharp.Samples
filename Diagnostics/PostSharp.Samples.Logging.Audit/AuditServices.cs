using System;

namespace PostSharp.Samples.Logging.Audit
{
    /// <summary>
    /// Exposes a <see cref="RecordPublished"/> event, which is raised whenever an audited method is executed.
    /// </summary>
    public static class AuditServices
    {
        internal static void PublishRecord(AuditRecord currentRecord)
        {
            RecordPublished?.Invoke(null, new AuditRecordEventArgs(currentRecord));
        }

        /// <summary>
        /// Event raised whenever an audited method is executed.
        /// </summary>
        public static event EventHandler<AuditRecordEventArgs> RecordPublished;
    }
}
