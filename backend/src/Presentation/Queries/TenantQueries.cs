using Core.Features.Tenants.Queries;
using Core.Features.Tenants.Queries.Dtos.Responses;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TenantQueries
{
    public Task<GetAllTenantsResponse> GetAllTenantsAsync([Service] IMediator mediator)
    {
        return mediator.Send(new GetAllTenants.Query());
    }

    public Task<TenantDto> GetTenantConfigurationAsync([Service] IMediator mediator, int code)
    {
        return mediator.Send(new GetTenantConfiguration.Query(code));
    }
}