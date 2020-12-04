using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Adapters.AspNetCore;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using PostSharp.Patterns.Diagnostics.Correlation;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using Serilog;
using Serilog.Sinks.Elasticsearch;
using System;
using System.Reflection;

[assembly: Log]

namespace MicroserviceExample
{
  public class Program
  {


    public static void Main(string[] args)
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



        // Defines a filter that selects trusted requests. 
        // Enabling HTTP Correlation Protocol for communication with untrusted devices is a security risk.
        Predicate<CorrelationRequest> trustedRequests = request => request.RemoteHost == "localhost" || 
                                                                   request.RemoteHost == "127.0.0.1" ||
                                                                   request.RemoteHost == "::1";

        // Instrument ASP.NET Core.
        AspNetCoreLogging.Initialize( correlationProtocol: new LegacyHttpCorrelationProtocol(trustedRequests) );


        // Execute the web app.
        CreateWebHostBuilder(args).Build().Run();

      }
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseStartup<Startup>();




  }

}
