This example demonstrates how to use PostSharp Framework in Blazor WebAssembly applications.

The application contains one custom `AutoRetry` aspect which is applied to the `WeatherService` class.
The `AutoRetry` aspect, when applied to a method, retries the execution of the method when the previous execution results in an exception.

For the purpose of this example the `WeatherService.GetCurrentForecast()` method simulates a connection failure on 50% of all requests.

The `LinkerConfig.xml` file is required to prevent compatibility issues between the Blazor linker and PostSharp in the Release build.