using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Adapters.HttpClient;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using PostSharp.Patterns.Diagnostics.Custom;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using Serilog;
using Serilog.Sinks.Elasticsearch;
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
      

      // Configure Serilog to write to the console and to Elastic Search.
      using (var logger = new LoggerConfiguration()
          .Enrich.WithProperty("Application", "Client")
          .MinimumLevel.Debug()
           .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
           {
             BatchPostingLimit = 1, // For demo.
             AutoRegisterTemplate = true,
             AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6,
             EmitEventFailure = EmitEventFailureHandling.ThrowException | EmitEventFailureHandling.WriteToSelfLog,
             FailureCallback = e => Console.WriteLine("Unable to submit event " + e.MessageTemplate),
           })
          .WriteTo.Console(
              outputTemplate:
              "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Indent:l}{Message:l}{NewLine}{Exception}")
          .CreateLogger())
      {

        // Configure PostSharp Logging to write to Serilog.
        var backend = new SerilogLoggingBackend(logger);
        backend.Options.IncludeActivityExecutionTime = true;
        backend.Options.IncludeExceptionDetails = true;
        backend.Options.SemanticParametersTreatedSemantically = SemanticParameterKind.All;
        backend.Options.IncludedSpecialProperties = SerilogSpecialProperties.All;
        backend.Options.ContextIdGenerationStrategy = ContextIdGenerationStrategy.Hierarchical;
        LoggingServices.DefaultBackend = backend;

        // Intercept outgoing HTTP requests and add logging to them.
        HttpClientLogging.Initialize(uri => uri.Port != 9200);


        using (logSource.Debug.OpenActivity(Formatted("Running the client"),
          new OpenActivityOptions(new Baggage { User = "Gaius Julius Caesar" })))
        {
          await QueueProcessor.ProcessQueue(".\\My\\Queue");
        }
      }

      Console.WriteLine("Done!");
    }

    [Log(AttributeExclude = true)]
    class Baggage
    {
      [LoggingPropertyOptions(IsBaggage =true)]
      public string User
      {
        get; set;
      }
    }
  }
}