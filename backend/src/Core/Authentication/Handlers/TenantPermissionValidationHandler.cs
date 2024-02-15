using Core.Authentication.Attributes;
using Core.Authentication.Claims;
using Core.Authentication.UserAccessor;

namespace Core.Authentication.Handlers;

public class TenantPermissionAttributeHandler : IAttributeHandler<NeedsTenantPermissionAttribute>
{
    public Task HandleAsync<TRequest>(
        TRequest request,
        IUserAccessor userAccessor,
        object propertyValue,
        CancellationToken cancellationToken)
        where TRequest : notnull
    {
        var availableTenants = userAccessor.ClaimsPrincipal.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.AvailableTenants)?.Value;

        if (availableTenants is null or "")
        {
            throw new UnauthorizedAccessException("User does not have permission for the specified tenant.");
        }

        var tenantCodes = availableTenants.Split(',');

        if (!tenantCodes.Contains(propertyValue.ToString()))
        {
            throw new UnauthorizedAccessException("User does not have permission for the specified tenant.");
        }

        return Task.CompletedTask;
    }
}