using Core.Authentication.Attributes;
using Core.Features.Tenants.Queries.Dtos.Responses;
using Core.Features.Tenants.Queries.Extensions;

namespace Core.Features.Tenants.Queries;

public static class GetAllTenants
{
    [NeedsAdminPermission]
    public record Query : IRequest<GetAllTenantsResponse>;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Query, GetAllTenantsResponse>
    {
        public async Task<GetAllTenantsResponse> Handle(Query request, CancellationToken cancellationToken)
        {
            var tenants = await tenantRepository.GetAllAsync(cancellationToken);

            var result = tenants.ToDto();
            return result;
        }
    }
}