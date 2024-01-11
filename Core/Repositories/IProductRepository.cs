namespace Core.Repositories;

public class ProductFilter
{
    public ProductFilter(int tenantCode)
    {
        TenantCode = tenantCode;
    }
    
    public ProductFilter(int tenantCode, IEnumerable<string>? tagCodes = null)
    {
        TenantCode = tenantCode;
        TagCodes = tagCodes;
    }
    
    public ProductFilter(int tenantCode, string? tagCode = null)
    {
        TenantCode = tenantCode;
        TagCodes = tagCode is null ? null : new[] { tagCode };
    }

    public int TenantCode { get; init; }

    public IEnumerable<string>? TagCodes { get; init; } = default!;
}

public interface IProductRepository
{
    public Task CreateAsync(Product product, CancellationToken cancellationToken);

    public Task<Product?> GetByAsync(int tenantCode, string productName, CancellationToken cancellationToken);
    
    public Task<IEnumerable<Product>> GetAllAsync(ProductFilter productFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsByNameAsync(int tenantCode, string productName, CancellationToken cancellationToken);
    
    public Task UpdateAsync(string oldProductName, Product product, CancellationToken cancellationToken);
}