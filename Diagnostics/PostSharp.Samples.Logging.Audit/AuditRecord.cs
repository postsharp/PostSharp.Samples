// Copyright (c) SharpCrafters s.r.o. This file is not open source. It is released under a commercial
// source-available license. Please see the LICENSE.md file in the repository root for details.

using System;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Audit;

namespace PostSharp.Samples.Logging.Audit
{
    /// <summary>
    /// Represents an audit record published by the <see cref="AuditServices.RecordPublished"/> event.
    /// </summary>
    /// <remarks>
    /// <para>
    /// You can customize this class by inheritance. In this case, you also need to customize the <see cref="AuditBackend"/> and
    /// <see cref="AuditRecordBuilder"/> classes.
    /// </para>
    /// </remarks>
    public class AuditRecord 
    {
        /// <summary>
        /// Initializes a new <see cref="AuditRecord"/>.
        /// </summary>
        /// <param name="declaringType">Declaring type of the audited method.</param>
        /// <param name="memberName">Name of the audited method.</param>
        /// <param name="recordKind">Kind of record (typically <see cref="LogRecordKind.MethodSuccess"/> or <see cref="LogRecordKind.MethodException"/>).</param>
        public AuditRecord( Type declaringType, string memberName, LogRecordKind recordKind)
        {
            this.MemberName = memberName;
            this.RecordKind = recordKind;
            this.DeclaringType = declaringType;
            this.Time = DateTime.Now;
        }

        /// <summary>
        /// Gets the time when the execution ended.
        /// </summary>
        public DateTime Time { get; }

        /// <summary>
        /// Gets the declaring type of the method.
        /// </summary>
        public Type DeclaringType { get; }

        /// <summary>
        /// Gets the name of the method.
        /// </summary>
        public string MemberName { get; }

        /// <summary>
        /// Gets the kind of record (typically <see cref="LogRecordKind.MethodSuccess"/> or <see cref="LogRecordKind.MethodException"/>). 
        /// </summary>
        public LogRecordKind RecordKind { get;  }

        /// <summary>
        /// Gets a string describing the operation (typically, the method type, name, and parameters).
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets the <see cref="Exception"/> if the method failed, or <c>null</c> if the method succeeded.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets the instance on which the method was executed, or <c>null</c> if the method is a static method.
        /// </summary>
        public object Target { get; set; }
    }

}
