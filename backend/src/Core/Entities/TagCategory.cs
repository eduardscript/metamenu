namespace Core.Entities;

public class TagCategory(
    int tenantCode,
    string code)
{
    public int TenantCode { get; set; } = tenantCode;

    public string Code { get; set; } = code;
}