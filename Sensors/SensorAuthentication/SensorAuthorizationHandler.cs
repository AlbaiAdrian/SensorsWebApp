using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Primitives;

namespace Sensors.SensorAuthentication;

public class SensorAuthorizationHandler : AuthorizationHandler<SensorAuthorizationRequirement>
{
    private readonly ISensorValidator _sensorValidator;

    public SensorAuthorizationHandler(ISensorValidator sensorValidator)
    {
        _sensorValidator = sensorValidator;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, SensorAuthorizationRequirement requirement)
    {
        HttpContext httpContext = context.Resource as HttpContext;
        if (httpContext == null)
        {
            context.Fail();
        }

        StringValues clientKey, zoneKey, sensorKey;

        httpContext.Request.Headers.TryGetValue("X-ClientKey", out clientKey);
        httpContext.Request.Headers.TryGetValue("X-ZoneKey", out zoneKey);
        httpContext.Request.Headers.TryGetValue("X-SensorKey", out sensorKey);

        if (!_sensorValidator.Validate(clientKey, zoneKey, sensorKey))
        {
            context.Fail();
        }
        else
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
