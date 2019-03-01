This example demonstrates how to build an aspect `ProfileAttribute` that you can apply to any method to measure 
its execution time. It also shows how to aggregate the metrics inside the application and how to periodically
upload them to an Application Performance Monitoring system - here, Microsoft Application Insights.

## Metrics

The aspect measures a few key metrics:

 * *CPU Time* is the time that a method has been executing on the CPU. If the method calls `Thread.Sleep`, the
   sleeping time is not included in this metric.
 
 * *Thread Time* is the time when a method has been executed synchronously on a thread. For `async` methods, 
    the synchronous execution time is the wall time minus any time spent awaiting for another task.  
	If the method calls `Thread.Sleep`, the  sleeping time *is* included in this metric. However, if an async
	method calls `await Task.Delay`, this time is *not* included.
    
 * *Async Time* is the time between the beginning and the end of a method. For non-async methods, this metric
	equals the Thread Time. If a method calls `Thread.Sleep` or `await Task.Delay`, this time *is* included.

All the metrics above also include the time spent in children methods.

Additionally, this example computes *exclusive* metrics, i.e. the CPU and Thread times spent in the profiled
method, minus the time spent in any other profiled method called by the main method. If you want to compute
the amount of time spent inside a type, it is safe to add the exclusive metrics. However, surpringly enough,
exclusive time can be imprecise (and even negative) for any specific time sample. This happens when children
methods have completed at the end of the sample, but the parent method has not. The data for the child methods
are counted negatively, but the data for the parent has not yet been counted positively, which can result in
negative values. This imprecision gets smaller when the samples are longer, or when several samples are aggregated. 

## Performance design

The architecture of this example is optimized to have minimal impact on the profiled code. The profiled code
can execute without locks and even without waiting. To achieve this, every thread maintains its own instance
of the metrics. The collection thread then periodically aggregates the data of all threads. When the thread-local 
metric data has been changed while it was being read by the publishing thread, the publisher will simply retry
to read the data. To detect changes, a version number is incremented by the writer. No lock or interlocked
operation is required - only strong ordering of write accesses, which guaranteed by the AMD64 architecture
and is enforeced by a memory barrier on other architectures.

## Use of unofficial PostSharp features

This example uses an undocumented and officially unsupported feature called *slim aspects*, which allow
to execute aspects without allocating memory. These features are used internally by PostSharp.Patterns.*, 
but are not completely tested. You can use them in your projects, but bugs are not guaranteed to be solved.
   
## Application Insights

To see the data in Application Insights:

1. Change the instrumentation key in `Program.cs`.
2. Go to your Application Insights resource in the Azure Portal, and in the Overview page, click on the
   *Analytics* report.
3. Try a query like this:

```

customEvents
| limit 500
| project  
    timestamp,
    methodName = name, 
    avgAsyncTime = todouble(customMeasurements.AsyncTime) / todouble(customMeasurements.ExecutionCount),
    avgCpuTime = todouble(customMeasurements.CpuTime) / todouble(customMeasurements.ExecutionCount),
    avgThreadTime = todouble(customMeasurements.ThreadTime) / todouble(customMeasurements.ExecutionCount),
    avgThreadUse = todouble(customMeasurements.CpuTime) / todouble(customMeasurements.ThreadTime),
    exceptionsPerSec = todouble(customMeasurements.ExceptionCount) / todouble(customMeasurements.SampleTime) * 1000,
    executionsPerSec = todouble(customMeasurements.ExecutionCount) / todouble(customMeasurements.SampleTime) * 1000

```


## Limitations

The code has not been tested.


