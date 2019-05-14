# PostSharp.Samples.Logging.ElasticStack: ClientExample

This is the client side of the PostSharp.Samples.Logging.ElasticStack example.

The *InstrumentOutgoingRequestsAspect* aspect is the most interesting part of this project. It instruments the HttpClient class. It basically does three things:

* Open a client-side activity (aka context), which emits log client-side records into Elastic Search.
* Set the `Request-Id` header of the outgoing HTTP request to the `SyntheticId` of the newly created activity.
* Set the `Correlation-Context` header ot the outgoing HTTP request to the baggage (i.e. cross-process properties). In this example, we've set a cross-process property named `User` in `Program.Main`.