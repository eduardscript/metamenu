using Core.Authentication.Attributes;
using Core.Exceptions.Tenants;
using Core.Features.Tenants.Shared;

namespace Core.Features.Tenants.Queries;

public class GetTenantByTenantCode
{
    [NeedsAdminPermission]
    public record Query : IRequest<TenantDto>
    {
        public int TenantCode { get; init; }
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Query, TenantDto>
    {
        public async Task<TenantDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenant = await tenantRepository.GetAsync(request.TenantCode, cancellationToken);
            
            if (tenant is null)
            {
                throw new TenantNotFoundException(request.TenantCode);
            }

            return tenant.ToDto();
        }
    }
}