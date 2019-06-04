using PostSharp.Patterns.Threading;
using System;
using System.Threading.Tasks;

namespace PostSharp.Samples.Threading.PingPong
{
  [Actor]
  internal class ConsoleLogger
  {
    [Reentrant]
    public async void WriteLine(string message, ConsoleColor color = ConsoleColor.White)
    {
      Console.ForegroundColor = color;
      Console.WriteLine(message);
    }

    [Reentrant]
    public async Task Flush()
    {
    }
  }
}