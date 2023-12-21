namespace Core.Features.Tenants.Queries;

public static class GetAllTenants
{
    public record Query : IRequest<IEnumerable<TenantDto>>;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Query, IEnumerable<TenantDto>>
    {
        public async Task<IEnumerable<TenantDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenants = await tenantRepository.GetAllAsync(cancellationToken);

            return tenants.Select(tenant => new TenantDto(tenant.TenantCode, tenant.Name));
        }
    }

    public record TenantDto(int TenantCode, string Name);
}