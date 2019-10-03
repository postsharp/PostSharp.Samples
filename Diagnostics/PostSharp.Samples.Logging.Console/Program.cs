using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;
using PostSharp.Samples.Logging.BusinessLogic;

// Add logging to all methods of the current project.
[assembly: Log]

namespace PostSharp.Samples.Logging.Console
{
  [Log(AttributeExclude = true)]   // Removes logging from the Program class itself.
  internal class Program
  {
    private static void Main(string[] args)
    {
      // Configure PostSharp Logging to output logs to the console.
      LoggingServices.DefaultBackend = new ConsoleLoggingBackend();


      // Simulate some business logic.
      QueueProcessor.ProcessQueue(@".\Private$\SyncRequestQueue");
    }
  }
}