using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;

namespace PostSharp.Samples.AutoRetry
{
  internal static class Program
  {
    private const double failureRate = 0.1;
    private static readonly Random random = new Random();
    private static readonly Stopwatch stopwatch = Stopwatch.StartNew();

    private static void Main(string[] args)
    {
      try
      {
        DownloadFile(
          "https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pineisland_oli_2017026_lrg-crop.jpg");
      }
      catch
      {
        Console.WriteLine("DownloadFile failed!");
      }

      try
      {
        DownloadFileAsync(
            "https://www.nasa.gov/sites/default/files/styles/full_width_feature/public/thumbnails/image/pineisland_oli_2017026_lrg-crop.jpg")
          .Wait();
      }
      catch
      {
        Console.WriteLine("DownloadFileAsync failed!");
      }
    }


    [AutoRetry(MaxRetries = 3)]
    private static void DownloadFile(string url)
    {
      WriteMessage("Attempting to download the file.");

      var webClient = new WebClient();
      var buffer = new byte[16 * 1024];


      using (var stream = webClient.OpenRead(url))
      {
        int countRead;
        while ((countRead = stream.Read(buffer, 0, buffer.Length)) != 0)
        {
          // Simulate a network failure.
          if (random.NextDouble() < failureRate)
          {
            WriteMessage("Network failure.");
            throw new WebException();
          }
        }
      }

      // Simulate success.
      WriteMessage("Success!");
    }

    [AutoRetry(MaxRetries = 3)]
    private static async Task DownloadFileAsync(string url)
    {
      WriteMessage("Attempting to download the file.");

      var webClient = new WebClient();
      var buffer = new byte[16 * 1024];


      using (var stream = webClient.OpenRead(url))
      {
        int countRead;
        while ((countRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
        {
          // Simulate a network failure.
          if (random.NextDouble() < failureRate)
          {
            WriteMessage("Network failure.");
            throw new WebException();
          }
        }
      }

      // Simulate success.
      WriteMessage("Success!");
    }

    // Writes a message to the console with a timestamp.
    private static void WriteMessage(string message)
    {
      Console.WriteLine("{0} ms - {1}", stopwatch.ElapsedMilliseconds, message);
    }
  }
}