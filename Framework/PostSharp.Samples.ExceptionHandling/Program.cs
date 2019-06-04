using PostSharp.Samples.ExceptionHandling;
using System;

// Add the AddContextOnException aspect to all methods in the assembly.

[assembly: AddContextOnException]

namespace PostSharp.Samples.ExceptionHandling
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      MainCore();

      // Proofs that MainCore succeeded despite Finbonacci throwing an exception.
      Console.WriteLine("The program returns successfully.");
    }

    [ReportAndSwallowException]
    private static void MainCore()
    {
      // The Fibonacci method will fail with an exception, but the [ReportAndSwallowException] aspect
      // will swallow the exception and the MainCore method will succeed.
      Fibonacci(5);
    }

    public static int Fibonacci(int n)
    {
      if (n < 0)
      {
        throw new ArgumentOutOfRangeException();
      }

      if (n == 0)
      {
        return 0;
      }
      // The next lines are intentionally commented out to cause an exception:

      // if (n == 1)
      //    return 1;

      return Fibonacci(n - 1) + Fibonacci(n - 2);
    }
  }
}