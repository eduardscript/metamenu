using Core.Features.Tenants.Commands;
using Core.Features.Tenants.Queries;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TenantMutations
{
    public async Task<GetAllTenants.TenantDto> CreateTenant(
        [Service] IMediator mediator,
        CreateTenant.Command command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return new GetAllTenants.TenantDto(command.TenantCode, command.Name);
    }
}