Have you ever had the problem where your program misbehaves and you just do not have a debugger attached
at the moment? Or that some bug is appearing occasionally and you just don't know how to reproduce
such a case manually? The usual solution here is logging. But introducing logging into your code comes with
a lot of manual work and adds a lot of boilerplate. There is also the dilemma of deciding which logic
should be traced and at which level of detail.

All this pain is taken away with PostSharp.

When you apply the `[LogMethod]` aspect to a method, the method creates a log record when:

* the method is invoked,
* the method returns successfully,
* the method fails with an exception.

Additionally, you can apply the `[LogSetValue]` to a field or property to create a log record when the field or property is set.

## What is being demonstrated?

The example uses the following PostSharp features:

* The `OnMethodBoundaryAspect` class, the advices `OnEntry`, `OnSuccess`, `OnException` and context values `args.Arguments`, `args.Method`. 
* The `LocationInterceptionAspect` class, the advice `OnSetValue` and context value `args.Location` and `args.Value`. 
* Multicasting logging on all methods of the assembly (see `Program.cs`).
* Logging calls to an external method, here the class `System.Math` (see `Program.cs`).


## Limitations

This example is purely educational. It has the following limitations:

* Not optimized for run-time performance.
* Not thread-safe.
* Subject to infinite loops if you add logging to the `ToString` method.

If you need production-grade logging, you should consider using the
`[Log]` aspect from the `[PostSharp.Patterns.Diagnostics]` namespace.

The `[Log]` aspect works out of the box. It is highly optimized for run-time performance, multithreading and reentrance. 
You can set up logging in a few minutes without modifying your existing source code. 


See https://doc.postsharp.net/logging.


