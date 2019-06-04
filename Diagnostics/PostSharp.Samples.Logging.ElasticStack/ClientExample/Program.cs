using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using Serilog;
using System;
using System.Threading.Tasks;
using static PostSharp.Patterns.Diagnostics.FormattedMessageBuilder;

[assembly: Log]

namespace ClientExample
{
  public static class Program
  {
    private static readonly LogSource logSource = LogSource.Get();

    private static async Task Main()
    {
      using (var logger = new LoggerConfiguration()
          .Enrich.WithProperty("Application", "ClientExample")
          .MinimumLevel.Debug()
          .WriteTo.Async(a => a.DurableHttp(requestUri: "http://localhost:31311", batchPostingLimit: 5))
          .WriteTo.Console(
              outputTemplate:
              "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Indent:l}{Message:l}{NewLine}{Exception}")
          .CreateLogger())
      {
        var backend = new SerilogLoggingBackend(logger);
        backend.Options.IncludeActivityExecutionTime = true;
        backend.Options.IncludeExceptionDetails = true;
        backend.Options.SemanticParametersTreatedSemantically = SemanticParameterKind.All;
        backend.Options.IncludedSpecialProperties = SerilogSpecialProperties.All;
        backend.Options.ContextIdGenerationStrategy = ContextIdGenerationStrategy.Hierarchical;
        LoggingServices.DefaultBackend = backend;


        using (logSource.Debug.OpenActivity(Formatted("Running the client"), new OpenActivityOptions
        {
          Properties = new[]
            {
                        new LoggingProperty("User", "Gaius Julius Caesar") {IsBaggage = true}
                    }
        }))
        {
          await QueueProcessor.ProcessQueue(".\\My\\Queue");
        }
      }

      Console.WriteLine("Done!");
    }
  }
}