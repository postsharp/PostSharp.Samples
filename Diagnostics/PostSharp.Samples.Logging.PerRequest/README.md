This example demonstrates how to configure verbosity differently for each request using the XML configuration file [postsharp-logging.config](postsharp-logging.config).
output in the log.

The initialization code is simple:

```cs
 AspNetCoreLogging.Initialize();
LoggingServices.DefaultBackend = new ConsoleLoggingBackend();
LoggingServices.DefaultBackend.ConfigureFromXml(XDocument.Load("postsharp-logging.config"));
```

To start the example, do `dotnet run` in this directory.

Then open https://localhost:5001/ with your browser and read the instructions in the home page.

You will see that the home page is logged every 10 seconds but the Privacy page is logged every time.

