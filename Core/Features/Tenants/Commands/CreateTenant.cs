using Core.Features.Tenants.Queries;
using Core.Features.Tenants.Shared;

namespace Core.Features.Tenants.Commands;

public static class CreateTenant
{
    public record Command(
        int TenantCode,
        string Name) : IRequest<TenantDto>;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, TenantDto>
    {
        public async Task<TenantDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenant = new Tenant(
                request.Name,
                false);

            var newTenant = await tenantRepository.CreateAsync(tenant, cancellationToken);
            
            return new TenantDto(
                newTenant.TenantCode,
                newTenant.Name);
        }
    }
}