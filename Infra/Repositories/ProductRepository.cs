using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infra.Repositories;

public class ProductRepository(IMongoCollection<Product> collection) : IProductRepository
{
    public Task CreateAsync(Product product, CancellationToken cancellationToken)
    {
        return collection.InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string productName, CancellationToken cancellationToken)
    {
        return collection.Find(p => p.Name == productName).AnyAsync(cancellationToken);
    }
}