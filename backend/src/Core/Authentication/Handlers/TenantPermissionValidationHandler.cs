using Core.Authentication.Claims;

namespace Core.Authentication.Handlers;

public class TenantPermissionValidationHandler : IPermissionValidationHandler
{
    public Task HandleAsync<TRequest>(TRequest request,
        IUserAccessor userAccessor,
        object? propertyInfo,
        CancellationToken cancellationToken) where TRequest : notnull
    {
        if (propertyInfo is null)
        {
            return Task.CompletedTask;
        }

        var availableTenants = userAccessor.ClaimsPrincipal?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.AvailableTenants)?.Value;

        if (availableTenants is null or "")
        {
            throw new UnauthorizedAccessException("User does not have permission for the specified tenant.");
        }

        var tenantCodes = availableTenants.Split(',');

        if (!tenantCodes.Contains(propertyInfo.ToString()))
        {
            throw new UnauthorizedAccessException("User does not have permission for the specified tenant.");
        }

        return Task.CompletedTask;
    }
}
