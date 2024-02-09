using Core.Features.Tenants.Commands;
using Core.Features.Tenants.Shared;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TenantMutations
{
    public async Task<TenantDto> CreateTenant(
        [Service] IMediator mediator,
        CreateTenant.Command command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public Task<DeleteTenant.TenantDeletedDto> DeleteTenantAsync([Service] IMediator mediator,
        DeleteTenant.Command command)
    {
        return mediator.Send(command);
    }

    public Task<ToggleTenantStatus.TenantStatusDto> ToggleTenantStatusAsync([Service] IMediator mediator,
        ToggleTenantStatus.Command command)
    {
        return mediator.Send(command);
    }
}