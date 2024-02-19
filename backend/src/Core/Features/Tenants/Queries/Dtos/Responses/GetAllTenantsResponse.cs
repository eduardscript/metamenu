namespace Core.Features.Tenants.Queries.Dtos.Responses;

public class GetAllTenantsResponse(IEnumerable<TenantDto> tenants)
{
    public IEnumerable<TenantDto> Tenants { get; set; } = tenants;
}