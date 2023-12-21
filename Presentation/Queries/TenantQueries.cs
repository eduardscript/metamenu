using Core.Features.Tenants.Queries;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TenantQueries
{
    public Task<IEnumerable<GetAllTenants.TenantDto>> GetAllTenantsAsync([Service] IMediator mediator)
    {
        return mediator.Send(new GetAllTenants.Query());
    }
}