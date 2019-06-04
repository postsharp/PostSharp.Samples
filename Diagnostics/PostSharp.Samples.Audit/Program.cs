using PostSharp.Patterns.Diagnostics.Audit;
using System.Security.Principal;

namespace PostSharp.Samples.Audit
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      // Configure the auditing services.
      AuditServices.RecordPublished += OnAuditRecordPublished;

      // Simulate some audited business operation.
      var po = new PurchaseOrder();
      po.Approve();
    }

    private static void OnAuditRecordPublished(object sender, AuditRecordEventArgs e)
    {
      var record = new DbAuditRecord(
        WindowsIdentity.GetCurrent().Name,
        (BusinessObject) e.Record.Target,
        e.Record.MemberName,
        e.Record.Text
      );

      record.AppendToDatabase();
    }
  }
}