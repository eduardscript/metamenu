namespace Core.Entities;

public class Product(
    int tenantCode,
    string name,
    string? description,
    decimal price,
    IEnumerable<string> tagCodes)
{
    public int Code { get; set; }
    public int TenantCode { get; set; } = tenantCode;

    public string Name { get; set; } = name;

    public string? Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public IEnumerable<string> TagCodes { get; set; } = tagCodes;
}