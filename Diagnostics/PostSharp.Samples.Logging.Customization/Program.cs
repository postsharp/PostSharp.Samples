using PostSharp.Patterns.Diagnostics;
using PostSharp.Samples.Logging.BusinessLogic;

[assembly: Log]

namespace PostSharp.Samples.Logging
{
  [Log(AttributeExclude = true)]
  internal static class Program
  {
    private static void Main(string[] args)
    {
      // Register the custom logging backend.
      var backend = new CustomLoggingBackend();
      LoggingServices.DefaultBackend = backend;

      // Register the custom parameter formatter.
      LoggingServices.Formatters.Register(new FancyIntFormatter());


      // Simulate some business logic.
      QueueProcessor.ProcessQueue(@".\Private$\SyncRequestQueue");

      ExampleFormattable.Greet(new ExampleFormattable { FirstName = "Yuri", LastName = "Gagarin" });
    }
  }
}