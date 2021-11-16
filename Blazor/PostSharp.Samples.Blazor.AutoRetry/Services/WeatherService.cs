using PostSharp.Samples.AutoRetry.Aspects;
using PostSharp.Samples.Blazor.AutoRetry.Model;
using System.Net.Http.Json;

namespace PostSharp.Samples.Blazor.AutoRetry.Services
{
  [AutoRetry]
  public class WeatherService
  {
    private int counter;

    private readonly HttpClient httpClient;

    public WeatherService(HttpClient httpClient)
    {
      this.httpClient = httpClient;
    }

    public async Task<WeatherForecast[]?> GetCurrentForecast()
    {
      // Fail every other request.
      if (++counter % 2 == 1)
      {
        throw new HttpRequestException("Service unavailable.");
      }

      return await this.httpClient.GetFromJsonAsync<WeatherForecast[]>("sample-data/weather.json");
    }
  }
}
