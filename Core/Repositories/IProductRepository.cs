namespace Core.Repositories;

public class ProductFilter
{
    public int TenantCode { get; init; }

    public string? TagCategoryCode { get; init; } = default!;
}

public interface IProductRepository
{
    public Task CreateAsync(Product product, CancellationToken cancellationToken);

    public Task<IEnumerable<Product>> GetAllProducts(ProductFilter productFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsByNameAsync(string productName, CancellationToken cancellationToken);
}