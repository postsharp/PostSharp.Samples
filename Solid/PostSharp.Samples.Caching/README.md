This example demonstrates the use of PostSharp.Patterns.Caching.

## Cache with direct invalidation

In the `CustomerService` class, we added the `[Cache]` attribute to the `GetCustomer` method, which
automatically caches the result of the method. The cache key is built from the type name, the method name, and the
method parameter `id`. In this class, the caching policy is set by the `[CacheConfiguration]` attribute on
the `CustomerService` class. Alternatively, we could have set the policy using properties of the `[Cache]` attribute
itself. The advantage of using the class-level `[CacheConfiguration]` attribute is that all cached method in
the class will share that configuration setting.

Caching stuff is generally easy, but removing from the cache is generally much harder. We call this *cache invalidation*.

The `[InvalidateCache(nameof(GetCustomer))]` attribute on the top of the `UpdateCustomer` method means that
the result of the `GetCustomer` method will be removed from the cache, for the specific value of the `id` parameter,
after the `UpdateCustomer` methodhas  successfully executed. Arguments of `GetCustomer` are matched at build-time
against those of `UpdateCustomer` and you will get a build-time error in case of mismatch. Therefore the use
of the `[InvalidateCache]` attribute is type safe.

The `DeleteCustomer` method shows how to perform cache invalidation imperatively by calling the
`CachingServices.Invalidation.Invalidate` method. Calling this method is also type safe thanks to the use of
a strongly typed delegate to specify which method should be removed from the cache.

The inconvenience of these two approaches is that both require the update method to know exactly which
read method must be invalidated. This creates a bad separation of concerns between the read and the update layers
of your code and may cause maintenance issues.

## Indirect cache invalidation with dependencies

How to avoid to couple the read and update methods in regard to caching? How to ensure you don't have to modify
the code of update methods when you add a read method?

The solution is to add a layer of abstraction between the read and update methods. This layer is composed of
*cache dependencies*. PostSharp offers an object-oriented abstraction for cache dependencies. A dependency
is anything that implements the `ICacheDependency` interface. It has a single method: `GetCacheKey`, which
should return a string that uniquely identifies the dependency object.

Cache dependencies are illustrated in the `Account` and `AccountServices` classes.

The business object `Account` implements the `ICacheDependency` interface. The `AccountServices.GetAccount` method
returns an `Account` but also calls `CachingServices.CurrentContext.AddDependency(account)` to specify the
the `GetAccount` method result depends on the `Account` dependency.

When the `UpdateAccount` method calls `CachingServices.Invalidation.Invalidate(account)`, PostSharp will
remove the result of both the `GetAccount` methods with relevant arguments.

Now, suppose you add a read method called  `GetAccountsOfCustomer` method, which internally calls the `GetAccount` method.
Do you need to update `UpdateAccount`? No, because dependencies of each invocation of the `GetAccount` method are automatically added to dependencies of the `GetAccountsOfCustomer` method. Therefore, when the `UpdateAccount` method calls `CachingServices.Invalidation.Invalidate(account)`, PostSharp invalidates the result of both `GetAccount` and `GetAccountsOfCustomer`.

As you can see, the use of dependencies improves the separation of concerns (i.e. decreases the coupling) between the
reading and updating parts of your application.

However, dependencies come with some cost, as we will see in an instant.

Also note `[CacheConfiguration(ProfileName = "Account")]` on the `AccountServices` class. It says that the caching feature will
be configured at run-time with a profile named `Account`.

## Configuring the caching aspect at run time

Before you can start executing code enhanced with the `[Cache]` aspect, you need to configure the caching service. Most
importantly, you need to specify the implementation of the storage. In this example, we will use Redis.

The run-time configuration is done in the `Program.Main` method.

The `RedisServer.Start()` command starts a local Redis server. In a real application, you don't need to do this since
you will use a network server.

We then have to configure Redis as the cache storage:

```
var configuration = new RedisCachingBackendConfiguration
{
    IsLocallyCached = true,
    SupportsDependencies = true
};

CachingServices.DefaultBackend = RedisCachingBackend.Create(connection, configuration);
```

The `IsLocallyCached` property adds a local `MemoryCache` in front of the remote Redis server.

The `SupportsDependency` property adds support for dependencies (i.e. indirect invalidation). If you enable this dependencies
with Redis, you must make sure that at least one instance of the garbage collector process runs, at any time, even when
you application does not run. This is a significant burden. If you deploy your application in the cloud or in a farm, you
can have two instances running in different availability groups, to make sure that at least one runs at any moment.

The following code executes the garbage collection process:

```
using ( RedisCacheDependencyGarbageCollector.Create(connection, configuration) )
{
  // ...
}
```
 

Finally, you need to configure the `Account` caching profile that we used in the `AccountService` class.

```
CachingServices.Profiles["Account"].AbsoluteExpiration = TimeSpan.FromSeconds(10);
```

## Results

This example demonstrated the use of the caching aspect with Redis. It showed both direct invalidation
(both declarative and imperative) and indirect invalidation with dependencies.

If you run the code, you should see in the console output when cached methods are evaluated
and when they are skipped.

## Documentation 

 [Caching](https://doc.postsharp.net/caching)

