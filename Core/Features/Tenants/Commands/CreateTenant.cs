using Core.Features.Tenants.Queries;

namespace Core.Features.Tenants.Commands;

public static class CreateTenant
{
    public record Command(
        int TenantCode,
        string Name) : IRequest<GetAllTenants.TenantDto>;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, GetAllTenants.TenantDto>
    {
        public async Task<GetAllTenants.TenantDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenant = new Tenant(
                request.Name,
                false);

            var newTenant = await tenantRepository.CreateAsync(tenant, cancellationToken);
            
            return new GetAllTenants.TenantDto(
                newTenant.TenantCode,
                newTenant.Name);
        }
    }
}