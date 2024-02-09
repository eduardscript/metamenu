using Core.Exceptions.Tenants;
using Core.Features.Tenants.Shared;

namespace Core.Features.Tenants.Queries;

public class GetTenantConfiguration
{
    public record Query(int tenantCode) : IRequest<TenantDto>;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Query, TenantDto>
    {
        public async Task<TenantDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenant = await tenantRepository.GetAsync(request.tenantCode, cancellationToken);

            if (tenant is null)
            {
                throw new TenantNotFoundException(request.tenantCode);
            }

            return tenant.ToDto();
        }
    }
}