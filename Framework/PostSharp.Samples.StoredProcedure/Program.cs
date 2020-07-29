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

        api.SetSpeakerStatus(1, true);
        api.SetSpeakerStatus(2, true);

        // Try the async API.
        foreach (var speaker in api.GetActiveSpeakers())
        {
          Console.WriteLine(speaker);
        }

        await api.SetSpeakerStatusAsync(1, true);
        await api.SetSpeakerStatusAsync(2, false);

        foreach (var speaker in api.GetActiveSpeakers())
        {
          Console.WriteLine(speaker);
        }

        // TODO: GetActiveSpeakersAsync does not work because of a bug in PostSharp.

      }
    }
  }
}