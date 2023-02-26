using FluentValidation;
using RefitRestClient.Host.Models;

namespace RefitRestClient.Host.Validators;

public class WeatherApiOptionsValidator : AbstractValidator<WeatherApiOptions>
{
    public WeatherApiOptionsValidator()
    {
        RuleFor(options => options.BaseUrl)
            .NotEmpty()
            .MaximumLength(255);

        RuleFor(options => options.AppKeyName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(options => options.AppKeyValue)
            .NotEmpty()
            .MaximumLength(255);
    }
}
