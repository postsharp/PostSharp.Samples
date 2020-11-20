using System;

namespace PostSharp.Samples.Logging.Audit
{
  public class BusinessObject
  {
    public Guid Id { get; } = Guid.NewGuid();
  }
}