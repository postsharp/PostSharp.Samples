using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
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
      using (var logger = new LoggerConfiguration()
          .Enrich.WithProperty("Application", "PostSharp.Samples.Logging.ElasticStack.ClientExample")
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
        var backend = new SerilogLoggingBackend(logger);
        backend.Options.IncludeActivityExecutionTime = true;
        backend.Options.IncludeExceptionDetails = true;
        backend.Options.SemanticParametersTreatedSemantically = SemanticParameterKind.All;
        backend.Options.IncludedSpecialProperties = SerilogSpecialProperties.All;
        backend.Options.ContextIdGenerationStrategy = ContextIdGenerationStrategy.Hierarchical;
        LoggingServices.DefaultBackend = backend;


        using (logSource.Debug.OpenActivity(Formatted("Running the client"), 
          new OpenActivityOptions {  Properties = new[] { new LoggingProperty("User", "Gaius Julius Caesar") {IsBaggage = true} } }))
        {
          await QueueProcessor.ProcessQueue(".\\My\\Queue");
        }
      }

      Console.WriteLine("Done!");
    }
  }
}