using Core.Authentication.Attributes;
using Core.Authentication.Claims;

namespace Core.Authentication.Handlers;

public class AdminPermissionValidationHandler : IPermissionValidationHandler
{
    public Task HandleAsync<TRequest>(
        TRequest request,
        IUserAccessor userAccessor,
        object? propertyInfo,
        CancellationToken cancellationToken) where TRequest : notnull
    {
        var roles = userAccessor.ClaimsPrincipal?.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.Roles)?.Value;

        if (roles is not null)
        {
            var userRoles = roles.Split(',');

            if (!userRoles.Contains(Roles.Admin))
            {
                throw new UnauthorizedAccessException("User does not have permission for the specified tenant.");
            }
        }

        return Task.CompletedTask;
    }
}