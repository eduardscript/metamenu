using Core.Authentication.Attributes;
using Core.Exceptions.Tenants;
using Core.Features.Tenants.Queries.Dtos.Responses;
using Core.Features.Tenants.Queries.Extensions;

namespace Core.Features.Tenants.Queries;

public static class GetTenantConfiguration
{
    [NeedsAdminPermission]
    public record Query(int code) : IRequest<TenantDto>;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Query, TenantDto>
    {
        public async Task<TenantDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenant = await tenantRepository.GetByCodeAsync(request.code, cancellationToken);

            if (tenant is null)
            {
                throw new TenantNotFoundException(request.code);
            }

            var result = tenant.ToDto();
            return result;
        }
    }
}