This example demonstrates how to use both PostSharp Logging and the error monitoring service [elmah.io](https://elmah.io) in an ASP.NET Core application.

You will need to create an elmah.io account and use your API key and log id in the setup code in `Program.cs`

`Program.cs` contains initialization logic, and `Startup.cs` has an extra statement that adds additional properties to logged events in elmah.io.

The `[assembly: Log]` custom attribute in `Program.cs` adds logging to the whole project. 

We describe this project in some detail in a joint blog post with elmah.io [at our blog](https://blog.postsharp.net/elmah-io).