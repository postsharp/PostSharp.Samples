using log4net.Config;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Log4Net;
using PostSharp.Samples.Logging.BusinessLogic;
using System.IO;

// Add logging to all methods of this project.
[assembly: Log]


namespace PostSharp.Samples.Logging.Log4Net
{
  [Log(AttributeExclude = true)]   // Removes logging from the Program class itself.
  internal class Program
  {
    private static void Main(string[] args)
    {
      // Configure Log4Net
      XmlConfigurator.Configure(new FileInfo("log4net.config"));

      // Configure PostSharp Logging to use Log4Net
      var log4NetLoggingBackend = new Log4NetLoggingBackend();
      LoggingServices.DefaultBackend = log4NetLoggingBackend;

      // Simulate some business logic.
      QueueProcessor.ProcessQueue(@".\Private$\SyncRequestQueue");
    }
  }
}