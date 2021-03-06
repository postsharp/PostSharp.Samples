using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Adapters.AspNetCore;
using PostSharp.Patterns.Diagnostics.Backends.Console;

[assembly: Log]

namespace PostSharp.Samples.Logging.PerRequest
{
  public class Program
  {
    public static void Main(string[] args)
    {
      
      AspNetCoreLogging.Initialize();
      LoggingServices.DefaultBackend = new ConsoleLoggingBackend();

      // This loads the configuration file from disk. 
      // You can also store this file in a cloud storage service and use ConfigureFromXmlWithAutoReloadAsync
      // to load it and have it automatically reload every minute or so, so you can dynamically
      // change the verbosity.
     LoggingServices.DefaultBackend.ConfigureFromXml(XDocument.Load("postsharp-logging.config"));

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
