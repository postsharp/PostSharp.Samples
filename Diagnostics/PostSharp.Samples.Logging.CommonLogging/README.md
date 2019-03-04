This example demonstrates how to configure the PostSharp Logging aspect so that it prints its output to Common.Logging.
Here Common.Logging is configured to direct outputs to the system console.

`Program.cs` is the only interesting one file. The other classes are members of the `PostSharp.Samples.Logging.BusinessLogic`
namespace and only simulate some business logic.

The `[assembly: Log]` custom attribute in `Program.cs` adds logging to the whole project. However, the `[Log(AttributeExclude = true)]`
attribute on the `Program` class prevents the `Program` class itself from being logged. This is required because no logged
code is allowed before the logging service is initialized.

The first line of the `Program.Main` method initializes PostSharp Logging. The method then executes the business code to produce some
output in the log.

## Viewing the log

In this example, the log is only printed to the console. No file is created.


## Documentation

[Logging to the Common.Logging](https://doc.postsharp.net/common-logging)