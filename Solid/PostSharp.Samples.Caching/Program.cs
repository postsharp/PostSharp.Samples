using PostSharp.Patterns.Caching;
using PostSharp.Patterns.Caching.Backends.Redis;
using PostSharp.Patterns.Diagnostics;
using PostSharp.Patterns.Diagnostics.Backends.Console;
using StackExchange.Redis;
using System;
using System.Linq;

namespace PostSharp.Samples.Caching
{
  [CacheConfiguration(AbsoluteExpiration = 5)]
  internal class Program
  {
    private static void Main(string[] args)
    {

      AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
      LoggingServices.DefaultBackend = new ConsoleLoggingBackend();

      // Uncomment the next line for detailed logging.
      // LoggingServices.DefaultBackend.DefaultVerbosity.SetMinimalLevel(LogLevel.Debug, LoggingRoles.Caching);

      using (RedisServer.Start())
      {
        using (var connection = ConnectionMultiplexer.Connect("localhost:6380,abortConnect = False"))
        {

          connection.InternalError += (sender, eventArgs) => Console.Error.WriteLine(eventArgs.Exception);
          connection.ErrorMessage += (sender, eventArgs) => Console.Error.WriteLine(eventArgs.Message);
          connection.ConnectionFailed += (sender, eventArgs) => Console.Error.WriteLine(eventArgs.Exception);

          var configuration = new RedisCachingBackendConfiguration
          {
            IsLocallyCached = true,
            SupportsDependencies = true
          };

          using (var backend = RedisCachingBackend.Create(connection, configuration))
          {
            // With Redis, we need at least one instance of the collection engine.
            using (RedisCacheDependencyGarbageCollector.Create(connection, configuration))

            {
              CachingServices.DefaultBackend = backend;

              // Configure the Account caching profile used in the AccountServices class.
              CachingServices.Profiles["Account"].AbsoluteExpiration = TimeSpan.FromSeconds(10);

              // Testing direct invalidation.
              Console.WriteLine("Retrieving the customer for the 1st time should hit the database.");
              CustomerServices.GetCustomer(1);
              Console.WriteLine("Retrieving the customer for the 2nd time should NOT hit the database.");
              CustomerServices.GetCustomer(1);
              Console.WriteLine("This should invalidate the GetCustomer method.");
              CustomerServices.UpdateCustomer(1, "New name");
              Console.WriteLine("This should hit the database again because GetCustomer has been invalidated.");
              CustomerServices.GetCustomer(1);

              // Testing indirect invalidation (dependencies).
              Console.WriteLine("Retrieving the account list for the 1st time should hit the database.");
              AccountServices.GetAccountsOfCustomer(1);
              Console.WriteLine("Retrieving the account list for the 2nt time should NOT hit the database.");
              var accounts = AccountServices.GetAccountsOfCustomer(1);
              Console.WriteLine("This should invalidate the accounts");
              AccountServices.UpdateAccount(accounts.First());
              Console.WriteLine(
                "This should hit the database again because GetAccountsOfCustomer has been invalidated.");
              AccountServices.GetAccountsOfCustomer(1);

              Console.WriteLine("Done!");
            }
          }
        }
      }
    }

    private static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
    {
     
    }
  }
}