using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Serilog;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.ElmahIo;

// Add PostSharp Logging to all methods and properties in the entire application:
[assembly: Log]

namespace PostSharp.Samples.Logging.ElmahIo
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // Set up Serilog:
      const string formatString = @"{Timestamp:yyyy-MM-dd HH:mm:ss}[{Level:u3}] {Indent:l}{Message}{NewLine}{Exception}";
      Log.Logger =
        new LoggerConfiguration()
          .MinimumLevel.Debug() // Capture all logs (PostSharp by default logs most traces at the Debug level)
          .Enrich.FromLogContext() // Add information from the web request to Serilog (used by elmah.io)
          .WriteTo.ColoredConsole(outputTemplate: formatString) // Pretty formatting and indentation for console/file
          .WriteTo.File("log.log", outputTemplate: formatString)
          .WriteTo.ElmahIo(new ElmahIoSinkOptions(
            "YOUR_API_KEY", // Use key and ID from your elmah.io account
            new Guid("YOUR_LOG_ID")
            )
          {
              MinimumLogEventLevel = LogEventLevel.Warning // only send warnings and errors to elmah.io
          })
          .CreateLogger();
      
      // Set up PostSharp Logging:
      LoggingServices.DefaultBackend = new SerilogLoggingBackend(Log.Logger)
      {
        Options =
        {
          // Add exception stack traces to both detailed and elmah.io logs:
          IncludeExceptionDetails = true
        }
      }; 
      CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
      Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
          webBuilder.UseStartup<Startup>();
        });
  }
}