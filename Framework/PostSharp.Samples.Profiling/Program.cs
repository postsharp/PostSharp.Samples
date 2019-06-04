using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;

namespace PostSharp.Samples.Profiling
{
  [Profile]
  internal class Program
  {
    static volatile bool cancel;
    private static void Main(string[] args)
    {
      // TODO: Add your own instrumentation key.
      const string instrumentationKey = "4fe9fc01-23cc-40ee-94e1-6dc532d861b9";

      var telemetryConfiguration = new TelemetryConfiguration(instrumentationKey);
      var telemetryClient = new TelemetryClient(telemetryConfiguration);
      if (!telemetryClient.IsEnabled())
      {
        Console.Write("TelemetryClient is not enabled.");
        return;
      }
      var period = TimeSpan.FromSeconds(10);
      ProfilingServices.Initialize(telemetryClient, period);

      Console.WriteLine($"Sampling every {period.TotalSeconds} seconds. Press Ctrl-C to stop, then wait a few seconds for completion.");

      Console.CancelKeyPress += OnCancel;

      var threads = new Thread[16];
      for (var i = 0; i < threads.Length; i++)
      {
        threads[i] = new Thread(ThreadMain);
        threads[i].Start();
      }

      foreach (var thread in threads)
      {
        thread.Join();
      }

      telemetryClient.Flush();
      Console.WriteLine("Done");

    }

    private static void OnCancel(object sender, ConsoleCancelEventArgs e)
    {
      Console.WriteLine("Cancelling. Please wait until completion, otherwise the Application Insights buffer won't be flushed.");
      cancel = true;
      e.Cancel = true;
    }

    private static void ThreadMain()
    {
      while (!cancel)
      {
        MainCore();
      }

    }

    private static void MainCore()
    {
      GetRandomBytes();
      SleepSync();
      SleepAsync().GetAwaiter().GetResult();
      ReadAndHashSync();
      ReadAndHashAsync().GetAwaiter().GetResult();
    }


    private static void SleepSync()
    {
      Thread.Sleep(200);
    }

    private static async Task SleepAsync()
    {
      await Task.Delay(200);
    }

    private static void GetRandomBytes()
    {
      for (var i = 0; i < 100; i++)
      {
        var generator = RandomNumberGenerator.Create();
        var randomBytes = new byte[32 * 1024 * 1024];
        generator.GetBytes(randomBytes);
      }
    }

    private static void ReadAndHashSync()
    {
      var hashAlgorithm = HashAlgorithm.Create("SHA256");
      hashAlgorithm.Initialize();

      var webClient = new WebClient();
      var buffer = new byte[16 * 1024];
      using (var stream = webClient.OpenRead("https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Robida_-_Ali-baba_page1.jpg/2880px-Robida_-_Ali-baba_page1.jpg"))
      {
        int countRead;
        while ((countRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
          hashAlgorithm.ComputeHash(buffer, 0, countRead);
        }
      }
    }

    private static async Task ReadAndHashAsync()
    {
      var hashAlgorithm = HashAlgorithm.Create("SHA256");
      hashAlgorithm.Initialize();

      var webClient = new WebClient();
      var buffer = new byte[16 * 1024];
      using (var stream = webClient.OpenRead("https://upload.wikimedia.org/wikipedia/commons/thumb/a/a2/Robida_-_Ali-baba_page1.jpg/2880px-Robida_-_Ali-baba_page1.jpg"))
      {
        int countRead;
        while ((countRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        {
          hashAlgorithm.ComputeHash(buffer, 0, countRead);
        }
      }
    }
  }
}