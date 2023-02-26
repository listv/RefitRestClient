using System.ComponentModel.DataAnnotations;

namespace RefitRestClient.Host.Models;

public class WeatherApiOptions
{
    public const string SectionName = "WeatherApi";

    [Required(AllowEmptyStrings = false)] public string AppKeyName { get; set; }

    [Required(AllowEmptyStrings = false)] public string AppKeyValue { get; set; }

    [Required(AllowEmptyStrings = false)] public string BaseUrl { get; set; }
}
