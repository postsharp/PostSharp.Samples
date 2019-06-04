using System;
using System.Collections.Generic;

namespace PostSharp.Samples.Audit.Extended
{
  public class DbAuditRecord
  {
    public DbAuditRecord(string user, Guid businessObject, IEnumerable<Guid> relatedBusinessObjects, string method,
      string description)
    {
      User = user;
      PrimaryBusinessObjectId = businessObject;
      Method = method;
      Description = description;
      RelatedBusinessObjectIds = relatedBusinessObjects;
    }

    public Guid PrimaryBusinessObjectId { get; }

    public IEnumerable<Guid> RelatedBusinessObjectIds { get; }

    public string Method { get; }
    public string User { get; }
    public string Description { get; }
    public DateTimeOffset Time { get; } = DateTimeOffset.Now;

    public void AppendToDatabase()
    {
      Console.WriteLine(
        $"Writing to the database: {{PrimaryBusinessObjectId={PrimaryBusinessObjectId}, Operation={Method}, Description=\"{Description}\", User={User}}}.");
      foreach (var id in RelatedBusinessObjectIds)
      {
        Console.WriteLine($"Writing to database: correlation with BusinessObjectId={id}.");
      }
    }
  }
}