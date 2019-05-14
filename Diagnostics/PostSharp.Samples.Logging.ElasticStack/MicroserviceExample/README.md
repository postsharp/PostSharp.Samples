# PostSharp.Samples.Logging.ElasticStack: MicroserviceExample

This is the server side of the PostSharp.Samples.Logging.ElasticStack example.

There are two interesting artifacts in this project:

* *LoggingActionFilter* is an ASP.NET Action Filter, i.e. it runs before the control flow is given to your code in the controller classes. This action filter does the "opposite" of the *InstrumentOutgoingRequestsAspect* server-side aspect, i.e. it creates a new server-side logging activity (aka context) and assigns:

    * the `SyntheticParentId` property to the value of the `Request-Id` HTTP header sent by the client, and
    * custom properties based on the parsing of the `Correlation-Context` HTTP header. 

* *SampledLoggingActionFilter* is another ASP.NET Action Filter which enables logging for a random 10% subset of incoming requests. The default level, for the remaining 90% of requests, is set to Warning in `Program.Main`.