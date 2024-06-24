using IdentityProvider.Presentation.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace IdentityProvider.Presentation.Middlewares;

public class UtconnectAuthorizationMiddlewareResultHandler : IAuthorizationMiddlewareResultHandler
{
    private readonly AuthorizationMiddlewareResultHandler _defaultHandler = new();

    public async Task HandleAsync(RequestDelegate requestDelegate, HttpContext httpContext,
        AuthorizationPolicy authorizationPolicy, PolicyAuthorizationResult policyAuthorizationResult)
    {
        Endpoint? endpoint = httpContext.GetEndpoint();
        JsonAuthorizationAttribute? jsonHeader = endpoint?.Metadata.GetMetadata<JsonAuthorizationAttribute>();
        if (jsonHeader == null)
        {
            await _defaultHandler.HandleAsync(requestDelegate, httpContext, authorizationPolicy,
                policyAuthorizationResult);
            return;
        }

        if (policyAuthorizationResult.Challenged)
        {
            var message = "User Credentials Not Found";

            if (!string.IsNullOrEmpty(jsonHeader.Message))
                message = jsonHeader.Message;

            httpContext.Response.StatusCode = 401;
            httpContext.Response.ContentType = "application/json";
            string jsonResponse = JsonConvert.SerializeObject(new { error = message });

            await httpContext.Response.WriteAsync(jsonResponse);
            return;
        }

        if (policyAuthorizationResult.Forbidden)
        {
            var message = "Invalid User Credentials";

            if (!string.IsNullOrEmpty(jsonHeader.Message))
                message = jsonHeader.Message;

            httpContext.Response.StatusCode = 403;
            httpContext.Response.ContentType = "application/json";
            string jsonResponse = JsonConvert.SerializeObject(new { error = message });

            await httpContext.Response.WriteAsync(jsonResponse);
            return;
        }

        await _defaultHandler.HandleAsync(requestDelegate, httpContext, authorizationPolicy,
            policyAuthorizationResult);
    }
}