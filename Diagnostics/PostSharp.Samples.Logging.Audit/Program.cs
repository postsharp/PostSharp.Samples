using PostSharp.Patterns.Diagnostics;
using PostSharp.Samples.Logging.Audit.Audit;
using PostSharp.Samples.Logging.Audit.Audit.Backend;
using System.Security.Principal;

namespace PostSharp.Samples.Logging.Audit
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      // Configure the audit backend.
      LoggingServices.Roles["Audit"].Backend = new AuditBackend();

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