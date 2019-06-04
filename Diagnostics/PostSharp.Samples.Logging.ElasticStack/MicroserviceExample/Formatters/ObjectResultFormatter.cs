using Microsoft.AspNetCore.Mvc;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Formatters;

namespace MicroserviceExample.Formatters
{
  public class ObjectResultFormatter : Formatter<ObjectResult>
  {
    public override void Write(UnsafeStringBuilder stringBuilder, ObjectResult value)
    {
      if (value.Value != null)
      {
        LoggingServices.Formatters.Get(value.Value.GetType()).Write(stringBuilder, value.Value);
      }
      else
      {
        stringBuilder.Append("null");
      }
    }
  }
}