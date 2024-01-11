namespace Core.Features.Tenants.Shared;

public record TenantDto(
    int Code,
    string Name);

public static class TenantDtoExtensions
{
    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto(
            tenant.Code,
            tenant.Name);
    }
}