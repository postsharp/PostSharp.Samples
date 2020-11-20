namespace PostSharp.Samples.Logging.Audit
{
  public class PurchaseOrder : BusinessObject
  {

    // Not the Audit aspect on this method.
    [Audit]
    public void Approve(string comment = null)
    {
      // Details skipped.
    }
  }
}