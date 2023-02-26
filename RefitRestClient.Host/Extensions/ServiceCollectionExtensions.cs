using Refit;
using RefitRestClient.Host.Infrastructure.Weather;
using RefitRestClient.Host.Models;

namespace RefitRestClient.Host.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWeatherApi(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddTransient<AuthKeyHandler>();
        var weatherApiOptions = configuration
            .GetSection(WeatherApiOptions.SectionName)
            .Get<WeatherApiOptions>();

        services.AddRefitClient<IWeatherApi>()
            .ConfigureHttpClient(client => client.BaseAddress = new Uri(weatherApiOptions.BaseUrl))
            .AddHttpMessageHandler<AuthKeyHandler>();

        return services;
    }
}
