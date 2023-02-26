using FluentValidation;
using Refit;
using RefitRestClient.Host.Extensions;
using RefitRestClient.Host.Infrastructure.Weather;
using RefitRestClient.Host.Models;
using RefitRestClient.Host.Validators;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<IValidator<WeatherApiOptions>, WeatherApiOptionsValidator>();

builder.Services
    .AddOptions<WeatherApiOptions>()
    .Bind(builder.Configuration.GetSection(WeatherApiOptions.SectionName))
    .ValidateFluently()
    .ValidateOnStart();

// Add WeatherApi client
builder.Services.AddTransient<AuthKeyHandler>();
var weatherApiOptions = builder.Configuration
    .GetSection(WeatherApiOptions.SectionName)
    .Get<WeatherApiOptions>();

builder.Services.AddRefitClient<IWeatherApi>()
    .ConfigureHttpClient(client => client.BaseAddress = new Uri(weatherApiOptions.BaseUrl))
    .AddHttpMessageHandler<AuthKeyHandler>();
//builder.Services.AddWeatherApi(builder.Configuration);
//

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
