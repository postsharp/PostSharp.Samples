using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Adapters.HttpClient;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using PostSharp.Patterns.Diagnostics.Correlation;
using PostSharp.Patterns.Diagnostics.Custom;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;
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
          .Enrich.WithProperty("Application", typeof(Program).Assembly.GetName().Name)
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

        // Defines a filter that selects trusted requests for which the correlation protocol is enabled.
        // Enabling HTTP Correlation Protocol for communication with untrusted devices is a security risk.
        Predicate<CorrelationRequest> trustedRequests = request => request.RemoteHost == "localhost" ||
                                                                   request.RemoteHost == "127.0.0.1" ||
                                                                   request.RemoteHost == "::1";

        // Determines which requests will be logged. We exclude requests to Logstash so that we are not logging
        // the logging itself.
        Predicate<Uri> loggedRequests = uri => uri.Port != 9200;

        // Intercept outgoing HTTP requests and add logging to them.
        HttpClientLogging.Initialize(
          correlationProtocol: new LegacyHttpCorrelationProtocol(trustedRequests),
          loggedRequests);


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