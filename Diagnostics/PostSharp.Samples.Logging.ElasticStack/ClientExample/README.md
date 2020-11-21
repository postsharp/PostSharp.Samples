# PostSharp.Samples.Logging.ElasticStack: ClientExample

This is the client-side of the PostSharp.Samples.Logging.ElasticStack example.

A few interesting points:

* Serilog is configured to write to the console and to Elastic Search (actually logstash).
* PostSharp Logging is configured to write to Serilog.
* The call to `HttpClientLogging.Initialize()` adds logging to outgoing HTTP requests, but also adds the headers that are required for distributed logging.
* The top-level custom attribute `[assembly: Log]` adds logging to every single method in the current project.