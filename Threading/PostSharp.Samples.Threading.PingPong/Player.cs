using PostSharp.Patterns.Model;
using PostSharp.Patterns.Threading;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PostSharp.Samples.Threading.PingPong
{
  [Actor]
  internal class Player
  {
    [Reference] private readonly ConsoleLogger logger;

    private readonly string name;

    [Reference] private readonly Random random = new Random();

    private readonly double skills;

    private int counter;


    public Player(ConsoleLogger logger, string name, double skills)
    {
      this.logger = logger;
      this.name = name;
      this.skills = skills;
    }

    [Reentrant]
    public async Task<int> GetCounter()
    {
      return counter;
    }

    [Reentrant]
    public async Task<Player> Ping(Player peer, ConsoleColor color)
    {
      logger.WriteLine(
        string.Format("{0}.Ping( color={1} ) from thread {2}", name, color, Thread.CurrentThread.ManagedThreadId),
        color);

      if (random.NextDouble() <= skills)
      {
        counter++;
        return await peer.Ping(this, color);
      }
      return peer;
    }

    [ExplicitlySynchronized]
    public override string ToString()
    {
      // It is safe to return a read-only immutable field.
      return name;
    }
  }
}