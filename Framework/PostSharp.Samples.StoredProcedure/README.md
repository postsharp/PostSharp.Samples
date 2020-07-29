
This example implements something like P-Invoke, but it calls database stored procedures instead of native functions.

Methods have to be defined as `extern` like this:

```cs
[MethodImpl(MethodImplOptions.InternalCall)]
public extern IEnumerable<Speaker> GetActiveSpeakers();
```

In this design, methods must be instance (non-static) and declared in a class derived from `BaseDpApi`. This base class
defined the `Connection` and `Transaction` properties used by the aspect.

The `StoredProcedureAttribute` class is the implementation of the aspect itself. It is applied to the `BaseDbApi` class
with inheritance and multicast enabled, so any `extern` method in a derived class will be turned into a stored 
procedure call.

The `MethodImpl` thing is ugly but unfortunately required. 

To test this sample, you must create a SQL database, load `CreateDb.sql`, and change the connection string in `Program.Main`.

This code is given for inspiration. A production-ready implementation should add error handling, test mappings,
implement out parameters, and so on.

