namespace Core.Features.Tenants.Shared;

public record TenantDto(
    int Code,
    string Name);

public static class TenantDtoExtensions
{
    public static IEnumerable<TenantDto> ToDto(this IEnumerable<Tenant> tenants)
    {
        return tenants.Select(x => x.ToDto());
    }
    
    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto(
            tenant.Code,
            tenant.Name);
    }
}