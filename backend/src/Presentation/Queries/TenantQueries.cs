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

    public Task<TenantDto> GetTenantConfigurationAsync(
        [Service] IMediator mediator,
        GetTenantConfiguration.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}