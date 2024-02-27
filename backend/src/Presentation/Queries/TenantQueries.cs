using Core.Features.Tenants.Queries;
using Core.Features.Tenants.Shared;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TenantQueries
{
    public Task<IEnumerable<TenantDto>> GetAllTenantsAsync([Service] IMediator mediator)
    {
        return mediator.Send(new GetAllTenants.Query());
    }
    
    public Task<TenantDto> GetTenantByTenantCodeAsync([Service] IMediator mediator, int tenantCode)
    {
        return mediator.Send(new GetTenantByTenantCode.Query { TenantCode = tenantCode });
    }
}