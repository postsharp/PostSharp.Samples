namespace ClientExample
{
  public class QueueItem
  {
    public int Id;
    public Verb Verb;
    public string Value;

    public QueueItem(int id, Verb verb, string value = null)
    {
      this.Id = id;
      this.Verb = verb;
      this.Value = value;
    }



    public override string ToString()
    {
      return $"SyncRequest Verb={this.Verb}, Id={this.Id}, Value=\"{this.Value}\"";
    }
  }

  public enum Verb
  {
    Get,
    AddOrUpdate,
    Create,
    Delete
  }
}