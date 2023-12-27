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
        await mediator.Send(command, cancellationToken);

        return new TenantDto(command.TenantCode, command.Name);
    }
}