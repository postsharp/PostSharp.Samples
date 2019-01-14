using System;
using System.Threading.Tasks;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using PostSharp.Patterns.Diagnostics.RecordBuilders;
using Serilog;
using Serilog.Sinks.Http.BatchFormatters;

[assembly: Log]

namespace ClientExample
{
    public static class Program
    {
        private static async Task Main()
        {
            var logger = new LoggerConfiguration()
                .Enrich.WithProperty( "Application", "ClientExample" )
                .MinimumLevel.Debug()
                .WriteTo.Async( a => a.DurableHttp( requestUri: "http://localhost:31311", batchFormatter: new ArrayBatchFormatter(), batchPostingLimit: 5 ) )
                .WriteTo.Console( outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Indent:l}{Message:l}{NewLine}{Exception}" )
                .CreateLogger();


            var backend = new SerilogLoggingBackend( logger );
            LoggingServices.Roles["Custom"].CustomRecordLoggingOptions.IncludeExecutionTime = true;
            backend.Options.IncludeExceptionDetails = true;
            backend.Options.SemanticParametersTreatedSemantically |= SemanticParameterKind.MemberName;
            backend.Options.AddEventIdProperty = true;
            backend.Options.ContextIdGenerationStrategy = LoggingOperationIdGenerationStrategy.Hierarchical;
            LoggingServices.DefaultBackend = backend;

            using ( logger )
            {
                await QueueProcessor.ProcessQueue( ".\\My\\Queue" );
            }

            Console.WriteLine( "Done!" );
        }

    }
}
