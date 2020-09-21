using PostSharp.Samples.AutoRetry.Aspects;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace PostSharp.Samples.Blazor.AutoRetry.Services
{
  [AutoRetry]
  public class WeatherService
  {
    private int counter;
    private HttpClient httpClient;

    public WeatherService(HttpClient httpClient)
    {
      this.httpClient = httpClient;
    }

    public async Task<WeatherForecast[]> GetCurrentForecast()
    {
      // Fail every other request.
      if (++counter % 2 == 1)
      {
        throw new WebException("Service unavailable.");
      }

      return await this.httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
    }
  }

  public class WeatherForecast
  {
    public DateTime Date
    {
      get; set;
    }

    public int TemperatureC
    {
      get; set;
    }

    public string Summary
    {
      get; set;
    }

    public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);
  }
}
