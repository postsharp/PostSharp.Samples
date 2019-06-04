using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Audit;
using System;
using System.Collections.Generic;

namespace PostSharp.Samples.Audit.Extended
{
  public class ExtendedAuditRecord : AuditRecord
  {
    public ExtendedAuditRecord(Type declaringType, string memberName, LogRecordKind recordKind) : base(declaringType,
      memberName, recordKind)
    {
    }

    public List<BusinessObject> RelatedBusinessObjects { get; } = new List<BusinessObject>();
  }
}