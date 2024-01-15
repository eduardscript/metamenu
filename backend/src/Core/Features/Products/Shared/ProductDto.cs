namespace Core.Features.Products.Shared;

public class ProductDto(
    int code,
    int tenantCode,
    string name,
    string? description,
    decimal price,
    IEnumerable<string> tagCodes)
{
    public int Code { get; set; } = code;

    public int TenantCode { get; set; } = tenantCode;

    public string Name { get; set; } = name;

    public string? Description { get; set; } = description;

    public decimal Price { get; set; } = price;

    public IEnumerable<string> TagCodes { get; set; } = tagCodes;
}

public static class ProductDtoExtensions
{
    public static ProductDto ToDto(this Product product)
    {
        return new ProductDto(
            product.Code,
            product.TenantCode,
            product.Name,
            product.Description,
            product.Price,
            product.TagCodes);
    }
}