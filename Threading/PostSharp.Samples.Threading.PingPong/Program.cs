#region Copyright (c) 2004-2010 by SharpCrafters s.r.o.

// This file is part of PostSharp source code and is the property of SharpCrafters s.r.o.
// 
// Source code is provided to customers under strict non-disclosure agreement (NDA). 
// YOU MUST HAVE READ THE NDA BEFORE HAVING RECEIVED ACCESS TO THIS SOURCE CODE. 
// Severe financial penalties apply in case of non respect of the NDA.

#endregion

using System;
using System.Linq;
using System.Threading.Tasks;

namespace PostSharp.Samples.Threading.PingPong
{
  internal class Program
  {
    private static void Main()
    {
      AsyncMain().Wait();
    }

    private static async Task AsyncMain()
    {
      // Create a logger;
      var consoleLogger = new ConsoleLogger();

      // Create two players.
      var trump = new Player(consoleLogger, "Trump", 0.92);
      var clinton = new Player(consoleLogger, "Clinton", 0.9);

      // Start several concurrent games between players.
      ConsoleColor[] colors =
      {
        ConsoleColor.Cyan, ConsoleColor.Blue, ConsoleColor.Green, ConsoleColor.Magenta, ConsoleColor.Yellow,
        ConsoleColor.Red
      };
      var games = colors.Select(color => trump.Ping(clinton, color)).ToArray();

      // Wait for all games to finish.
      await Task.WhenAll(games);
      consoleLogger.WriteLine("We are all done");


      // Display winner and stats.
      for (var i = 0; i < games.Length; i++)
      {
        consoleLogger.WriteLine(string.Format("Winner for game {0}: {1}", colors[i], games[i].Result), colors[i]);
      }

      consoleLogger.WriteLine(string.Format("{0} totally received {1} balls", trump, await trump.GetCounter()));
      consoleLogger.WriteLine(string.Format("{0} totally received {1} balls", clinton, await clinton.GetCounter()));


      // Wait for the console logger.
      await consoleLogger.Flush();
    }
  }
}