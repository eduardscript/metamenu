using Core.Features.Tenants.Commands;
using Core.Features.Tenants.Commands.ConfigureTenantCommand;
using Core.Features.Tenants.Commands.ConfigureTenantCommand.Dtos.Responses;
using Core.Features.Tenants.Commands.CreateTenantCommand;
using Core.Features.Tenants.Commands.CreateTenantCommand.Dtos.Responses;
using Core.Features.Tenants.Commands.DeleteTenantCommand;
using Core.Features.Tenants.Commands.DeleteTenantCommand.Dtos.Responses;
using Core.Features.Tenants.Commands.ToggleTenantStatus;
using Core.Features.Tenants.Commands.ToggleTenantStatus.Dtos.Responses;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TenantMutations
{
    public async Task<CreateTenantResponse> CreateTenant(
        [Service] IMediator mediator,
        CreateTenant.Command command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public async Task<ConfigureTenantResponse> ConfigureTenant(
        [Service] IMediator mediator,
        ConfigureTenant.Command command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }

    public Task<DeleteTenantDto> DeleteTenantAsync([Service] IMediator mediator,
        DeleteTenant.Command command)
    {
        return mediator.Send(command);
    }

    public Task<ToggleTenantStatusResponse> ToggleTenantStatusAsync([Service] IMediator mediator,
        ToggleTenantStatus.Command command)
    {
        return mediator.Send(command);
    }
}