using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Xml;

namespace PostSharp.Samples.StoredProcedure
{
  internal class Program
  {
    public static async Task Main(string[] args)
    {
      using (SqlConnection connection = new SqlConnection(@"Data Source=.\SQLEXPRESS;Initial Catalog=PostSharpSamples;Integrated Security=True"))
      {
        await connection.OpenAsync();


        SpeakerApi api = new SpeakerApi(connection);


        Console.WriteLine("All speakers:");
        foreach (var speaker in api.GetSpeakers())
        {
          Console.WriteLine(speaker);
        }

        Console.WriteLine("Disable speaker 1...");
        await api.SetSpeakerStatusAsync(1, false);

        Console.WriteLine("Active speakers:");
        foreach (var speaker in api.GetActiveSpeakers())
        {
          Console.WriteLine(speaker);
        }

        // TODO: GetActiveSpeakersAsync does not work because of a bug in PostSharp.

      }
    }
  }
}