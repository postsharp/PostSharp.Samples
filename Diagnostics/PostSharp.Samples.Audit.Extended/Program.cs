using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Audit;
using System.Linq;
using System.Security.Principal;

namespace PostSharp.Samples.Audit.Extended
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      // Configure the auditing services.
      LoggingServices.Roles[LoggingRoles.Audit].Backend = new ExtendedAuditBackend();
      AuditServices.RecordPublished += OnAuditRecordPublished;

      // Simulate some audited business operation.
      var po = new PurchaseOrder();
      var employee = new Employee();
      po.AssignTo(employee);
    }

    private static void OnAuditRecordPublished(object sender, AuditRecordEventArgs e)
    {
      var extendedRecord = (ExtendedAuditRecord) e.Record;
      var record = new DbAuditRecord(
        WindowsIdentity.GetCurrent().Name,
        ((BusinessObject) extendedRecord.Target).Id,
        extendedRecord.RelatedBusinessObjects.Select(r => r.Id),
        extendedRecord.MemberName,
        extendedRecord.Text
      );

      record.AppendToDatabase();
    }
  }
}