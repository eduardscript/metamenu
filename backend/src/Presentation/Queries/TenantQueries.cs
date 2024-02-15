using Core.Features.Tenants.Queries;
using Core.Features.Tenants.Shared;
using HotChocolate.Authorization;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TenantQueries
{
    public Task<IEnumerable<TenantDto>> GetAllTenantsAsync([Service] IMediator mediator)
    {
        return mediator.Send(new GetAllTenants.Query());
    }
}