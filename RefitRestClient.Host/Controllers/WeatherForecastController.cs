using Microsoft.AspNetCore.Mvc;
using RefitRestClient.Host.Infrastructure.Weather;

namespace RefitRestClient.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IWeatherApi _weatherApi;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherApi weatherApi)
    {
        _logger = logger;
        _weatherApi = weatherApi;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IActionResult> Get(string location, CancellationToken cancellationToken = default)
    {
        var weatherForecast = await _weatherApi.Get(location, cancellationToken);

        return Ok(weatherForecast);
    }
}
