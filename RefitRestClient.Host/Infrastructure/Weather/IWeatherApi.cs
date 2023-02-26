using Refit;
using RefitRestClient.Host.Models;

namespace RefitRestClient.Host.Infrastructure.Weather;

public interface IWeatherApi
{
    [Get("/current.json")]
    Task<WeatherForecast> Get([Query] [AliasAs("q")] string location, CancellationToken cancellationToken = default);
}
