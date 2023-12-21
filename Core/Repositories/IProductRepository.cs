namespace Core.Repositories;

public interface IProductRepository
{
    public Task CreateAsync(Product product, CancellationToken cancellationToken);

    public Task<IEnumerable<Product>> GetAllProducts(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsByNameAsync(string productName, CancellationToken cancellationToken);
}