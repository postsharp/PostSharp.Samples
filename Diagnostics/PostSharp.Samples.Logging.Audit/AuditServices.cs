// Copyright (c) SharpCrafters s.r.o. This file is not open source. It is released under a commercial
// source-available license. Please see the LICENSE.md file in the repository root for details.

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
