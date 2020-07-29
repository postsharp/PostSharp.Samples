
using PostSharp.Community.ToString;

namespace PostSharp.Samples.StoredProcedure
{
  [ToString]
  internal class Speaker
  {
    public int Id
    {
      get; set;
    }

    public string Name
    {
      get; set;
    }

    public bool IsActive
    {
      get; set;
    }
  }
}
