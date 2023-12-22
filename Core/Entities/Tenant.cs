namespace Core.Entities;

public record Tenant(
    string Name,
    bool IsEnabled)
{
    public int TenantCode { get; init; }
}