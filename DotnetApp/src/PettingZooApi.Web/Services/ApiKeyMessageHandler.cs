using Microsoft.Extensions.Options;

namespace PettingZooApi.Web.Services;

public class ApiKeyMessageHandler : DelegatingHandler
{
    private readonly ApiKeyMessageHandlerOptions _options;
    public ApiKeyMessageHandler(IOptions<ApiKeyMessageHandlerOptions> options)
    {
        _options = options.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-Api-Key", _options.ApiKey);
        return await base.SendAsync(request, cancellationToken);
    }
}

public class ApiKeyMessageHandlerOptions
{
    public string ApiKey { get; set; } = string.Empty;
}