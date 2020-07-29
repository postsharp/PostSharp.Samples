# PostSharp.Samples

Welcome to this collection of PostSharp example projects.

You can [browse these samples online](https://samples.postsharp.net/) and navigate the code just by clicking on code references, 
or you can download them on [GitHub](https://www.github.com/postsharp/PostSharp.Samples).


| Project                                                                                                      | Description                                                                                  |
| :----------------------------------------------------------------------------------------------------------- | :------------------------------------------------------------------------------------------- |
| **Aspect Framework**                 
| [PostSharp.Samples.CustomLogging](Framework/PostSharp.Samples.CustomLogging/)                       | Logs method calls including parameter values.                                                |
| [PostSharp.Samples.CustomCaching](Framework/PostSharp.Samples.CustomCaching/)                       | Caches the results of methods calls                                                          |
| [PostSharp.Samples.ExceptionHandling](Framework/PostSharp.Samples.ExceptionHandling/)               | Add parameter values to call stack in exception details. Report and then swallow exceptions in entry points.  |
| [PostSharp.Samples.AutoRetry](Framework/PostSharp.Samples.AutoRetry/)                               | Automatically retries a method call when it fails.                                           | 
| [PostSharp.Samples.WeakEvent](Framework/PostSharp.Samples.WeakEvent/)                               | Prevents memory leaks due to events.                                                         | 
| [PostSharp.Samples.ValidateResourceString](Framework/PostSharp.Samples.ValidateResourceString/)     | Prints a build-time warning when incorrect resource string name is passed to parameter.      | 
| [PostSharp.Samples.SessionState](Framework/PostSharp.Samples.SessionState/)                         | Stores a field or property in the session state or page view state.                          | 
| [PostSharp.Samples.Transactions](Framework/PostSharp.Samples.Transactions/)                         | Automatically executes a method inside a transaction.                                        | 
| [PostSharp.Samples.Profiling](Framework/PostSharp.Samples.Profiling/)                               | Measure different execution times of methods, including async methods.                       | 
| [PostSharp.Samples.Encryption](Framework/PostSharp.Samples.Encryption/)                             | Automatically encrypts and decrypts parameter and fields/properties                          | 
| [PostSharp.Samples.MiniProfiler](Framework/PostSharp.Samples.MiniProfiler/)                         | Measures method execution time with MiniProfiler of StackExchange.                           | 
| [PostSharp.Samples.Persistence](Framework/PostSharp.Samples.Persistence/)                           | Persists fields or properties into the Windows registry or `app.config`.                     | 
| [PostSharp.Samples.AutoDataContract](Framework/PostSharp.Samples.AutoDataContract/)                 | Automatically adds `[DataContract]` and `[DataMember]` attributes to derived classes and all properties | 
| [PostSharp.Samples.Authorization](Framework/PostSharp.Samples.Authorization/)                       | Requires permissions before getting or setting fields or executing methods.                  | 
| [PostSharp.Samples.NormalizeString](Framework/PostSharp.Samples.NormalizeString/)                   | Trims and lowercases strings before they are assigned to a field or property.
| [PostSharp.Samples.StoredProcedure](Framework/PostSharp.Samples.StoredProcedure/)                   | Implements something like P-Invoke, but for database stored procedures.  |
| **Diagnostics**                 
| [PostSharp.Samples.Logging.Customization](Diagnostics/PostSharp.Samples.Logging.Customization/)     | Shows how to customize PostSharp Logging.                                                   | 
| [PostSharp.Samples.Logging.Console](Diagnostics/PostSharp.Samples.Logging.Console/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to the *system console*.  | 
| [PostSharp.Samples.Logging.Etw](Diagnostics/PostSharp.Samples.Logging.Etw/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to *ETW*.               | 
| [PostSharp.Samples.Logging.Etw.CustomSource](Diagnostics/PostSharp.Samples.Logging.Etw.CustomSource/)                 | Demonstrates how use PostSharp Logging with a custom *ETW* source (instead of the predefined one).               | 
| [PostSharp.Samples.Logging.Log4Net](Diagnostics/PostSharp.Samples.Logging.Log4Net/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to *log4net*.   | 
| [PostSharp.Samples.Logging.NLog](Diagnostics/PostSharp.Samples.Logging.NLog/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to *NLog*.   | 
| [PostSharp.Samples.Logging.Serilog](Diagnostics/PostSharp.Samples.Logging.Serilog/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to *Serilog*.   | 
| [PostSharp.Samples.Logging.Loupe](Diagnostics/PostSharp.Samples.Logging.Loupe/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to *Loupe*.   | 
| [PostSharp.Samples.Logging.CommonLogging](Diagnostics/PostSharp.Samples.Logging.CommonLogging/)                 | Demonstrates how to configure PostSharp Logging so that it directs its output to *Common.Logging*.   | 
| [PostSharp.Samples.Logging.CustomBackend.ServiceStack](Diagnostics/PostSharp.Samples.Logging.CustomBackend.ServiceStack/)  | Demonstrates how to implement a PostSharp Logging adapter for your custom logging framework.   | 
| [PostSharp.Samples.Logging.Audit](Diagnostics/PostSharp.Samples.Audit/)  | Shows how to append an audit record to a database when a method is invoked.   |
| [PostSharp.Samples.Logging.Audit.Extended](Diagnostics/PostSharp.Samples.Audit.Extended/)  | Shows how to add custom pieces of information to the audit record.   | 
| **Distributed Diagnostics with Elastic Search**                 
| [PostSharp.Samples.Logging.ElasticStack/MicroserviceExample](Diagnostics/PostSharp.Samples.Logging.ElasticStack/MicroserviceExample/)                 |  The server-side service demonstrating correlation of logs of a distributed application with Elastic Search. | 
| [PostSharp.Samples.Logging.ElasticStack/ClientExample](Diagnostics/PostSharp.Samples.Logging.ElasticStack/ClientExample/)                 |  The server-side service demonstrating correlation of logs of a distributed application with Elastic Search.  | 
| **XAML**                 
| [PostSharp.Samples.Xaml](Xaml/PostSharp.Samples.Xaml/)                                              | Demonstrates a few ready-made aspects that are useful for XAML                               | 
| **Caching**                 
| [PostSharp.Samples.Caching](Solid/PostSharp.Samples.Caching/)             | Caching method results with Redis and different ways to remove things from the cache. | 
| **Threading**                 
| [PostSharp.Samples.Threading.PingPong](Threading/PostSharp.Samples.Threading.PingPong/)             | The classic educational ping-pong example.                                                   | 
| [PostSharp.Samples.Threading.ThreadDispatching](Threading/PostSharp.Samples.Threading.ThreadDispatching/) | A simple WPF progress bar updated from a background thread.                            | 





 