This example demonstrates how to configure the PostSharp Logging aspect so that it prints its output to ETW.

`Program.cs` is the only interesting one file. The other classes are members of the `PostSharp.Samples.Logging.BusinessLogic`
namespace and only simulate some business logic.

The `[assembly: Log]` custom attribute in `Program.cs` adds logging to the whole project. However, the `[Log(AttributeExclude = true)]`
attribute on the `Program` class prevents the `Program` class itself from being logged. This is required because no logged
code is allowed before the logging service is initialized.

The first line of the `Program.Main` method initializes PostSharp Logging. The method then executes the business code to produce some
output in the log.

## Viewing the log

To view the log produced by this example, you need to attach a log collector:

1. Download PerfView from https://www.microsoft.com/en-us/download/details.aspx?id=28567.

2. Execute: perfview.exe collect -OnlyProviders:*PostSharp-Patterns-Diagnostics

3. Execute this program.

4. In PerfView, click 'Stop collecting', then in the PerfView tree view click 'PerfViewData.etl.zip' and finally 'Events'.


## Documentation

[Logging to the etw](https://doc.postsharp.net/etw)