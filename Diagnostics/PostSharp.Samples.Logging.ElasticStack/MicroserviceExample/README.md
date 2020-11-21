# PostSharp.Samples.Logging.ElasticStack: MicroserviceExample

This is the server-side of the PostSharp.Samples.Logging.ElasticStack example.

A few interesting points:

* Serilog is configured to write to the console and to Elastic Search (actually logstash).
* PostSharp Logging is configured to write to Serilog.
* The call to `AspNetCoreLogging.Initialize()` adds logging to the ASP.NET Core stack.
* The top-level custom attribute `[assembly: Log]` adds logging to every single method in the current project.