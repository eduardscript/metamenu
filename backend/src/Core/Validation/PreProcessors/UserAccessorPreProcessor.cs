using Core.Authentication;
using Core.Authentication.Attributes;
using Core.Authentication.Claims;
using MediatR.Pipeline;

namespace Core.Validation.PreProcessors;

public class UserAccessorPreProcessor<TRequest>(IUserAccessor userAccessor)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var propertiesWithTenantPermission = request.GetType()
            .GetProperties()
            .Where(prop => Attribute.IsDefined(prop, typeof(NeedsTenantPermissionAttribute)));

        foreach (var property in propertiesWithTenantPermission)
        {
            var tenantCode = property.GetValue(request);
            if (tenantCode is null)
            {
                continue;
            }

            var availableTenants = userAccessor.ClaimsPrincipal?.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.AvailableTenants)?.Value;

            if (availableTenants is not null)
            {
                var tenantCodes = availableTenants.Split(',');

                if (!tenantCodes.Contains(tenantCode.ToString()))
                {
                    throw new UnauthorizedAccessException("User does not have permission for the specified tenant.");
                }
            }
        }

        
        return Task.CompletedTask;
    }
}