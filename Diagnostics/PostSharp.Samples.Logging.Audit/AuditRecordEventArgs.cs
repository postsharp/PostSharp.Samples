// Copyright (c) SharpCrafters s.r.o. This file is not open source. It is released under a commercial
// source-available license. Please see the LICENSE.md file in the repository root for details.

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
