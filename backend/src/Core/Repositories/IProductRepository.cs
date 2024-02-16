namespace Core.Repositories;

public class ProductFilter(int tenantCode)
{
    public int TenantCode { get; init; } = tenantCode;

    public IEnumerable<string>? TagCodes { get; init; }

    public string? TagCode { get; set; }
}

public interface IProductRepository
{
    public class UpdateType
    {
        public string? TagCategoryCodeToRemove { get; init; }
    }

    public Task<Product> CreateAsync(Product product, CancellationToken cancellationToken);

    public Task<Product?> GetByAsync(int tenantCode, string productName, CancellationToken cancellationToken);

    public Task<IEnumerable<Product>> GetAllAsync(ProductFilter productFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsByNameAsync(int tenantCode, string productName, CancellationToken cancellationToken);

    public Task UpdateAsync(string oldProductName, Product product, CancellationToken cancellationToken);
    
    public Task<bool> UpdateManyAsync(ProductFilter productFilter, UpdateType updateType, CancellationToken cancellationToken);
}