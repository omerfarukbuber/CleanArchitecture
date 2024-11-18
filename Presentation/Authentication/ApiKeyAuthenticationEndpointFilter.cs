using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Presentation.Authentication;

internal sealed class ApiKeyAuthenticationEndpointFilter(IConfiguration configuration) : IEndpointFilter
{
    private const string ApiKeyHeader = "CA-ApiKey";
    private readonly IConfiguration _configuration = configuration;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var apiKey = context.HttpContext.Request.Headers[ApiKeyHeader];

        if (!IsApiKeyValid(apiKey))
        {
            return Results.Unauthorized();
        }

        return await next(context);
    }

    private bool IsApiKeyValid(string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            return false;
        }

        return _configuration.GetValue<string>("ApiKey") == apiKey;
    } 
}