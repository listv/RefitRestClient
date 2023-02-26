using System.Web;
using Microsoft.Extensions.Options;
using RefitRestClient.Host.Models;

namespace RefitRestClient.Host.Infrastructure.Weather;

public class AuthKeyHandler : DelegatingHandler
{
    private readonly WeatherApiOptions _options;

    public AuthKeyHandler(IOptions<WeatherApiOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var uriBuilder = new UriBuilder(request.RequestUri.AbsoluteUri);

        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);
        queryParams.Add(_options.AppKeyName, _options.AppKeyValue);

        uriBuilder.Query = queryParams.ToString();

        request.RequestUri = uriBuilder.Uri;

        return await base.SendAsync(request, cancellationToken);
    }
}
