namespace Core.Repositories;

public class ProductFilter
{
    public int TenantCode { get; init; }

    public IEnumerable<string>? TagCodes { get; init; } = default!;
}

public interface IProductRepository
{
    public Task CreateAsync(Product? product, CancellationToken cancellationToken);

    public Task<Product?> GetByAsync(int tenantCode, string productName, CancellationToken cancellationToken);
    
    public Task<IEnumerable<Product>> GetAllProducts(ProductFilter productFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsByNameAsync(int tenantCode, string productName, CancellationToken cancellationToken);
    
    public Task UpdateAsync(string oldProductName, Product product, CancellationToken cancellationToken);
}