using Core.Entities;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infra.Repositories;

public class ProductRepository(IMongoCollection<Product?> collection) : IProductRepository
{
    public async Task<Product> CreateAsync(Product product, CancellationToken cancellationToken)
    {
        var aggregateFluent = collection.Aggregate()
            .Group(new BsonDocument
            {
                { "_id", BsonNull.Value },
                { nameof(Product.Code), new BsonDocument("$max", "$Code") }
            })
            .Project<Product>(new BsonDocument
            {
                { "_id", 0 },
                { nameof(Product.Code), 1 }
            });

        var productCode = (await aggregateFluent.FirstOrDefaultAsync(cancellationToken))?.Code;

        product.Code = productCode is null ? 1 : productCode.Value + 1;

        await collection.InsertOneAsync(product, cancellationToken: cancellationToken);

        return product;
    }

    public Task<Product?> GetByAsync(int tenantCode, string productName, CancellationToken cancellationToken)
    {
        return collection
            .Find(p => p!.TenantCode == tenantCode && p.Name == productName)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllAsync(
        ProductFilter productFilter,
        CancellationToken cancellationToken)
    {
        var pipeline = new List<BsonDocument>
        {
            new("$match", new BsonDocument(nameof(Product.TenantCode), productFilter.TenantCode)),
        };

        if (productFilter.TagCodes is not null)
        {
            pipeline.Add(new("$match", new BsonDocument
            {
                {
                    nameof(Product.TagCodes), new BsonDocument
                    {
                        { "$in", new BsonArray(productFilter.TagCodes) }
                    }
                }
            }));
        }

        pipeline.Add(new("$project", new BsonDocument
        {
            { "_id", 1 },
            { nameof(Product.Code), 1 },
            { nameof(Product.TenantCode), 1 },
            { nameof(Product.Name), 1 },
            { nameof(Product.Description), 1 },
            { nameof(Product.Price), 1 },
            { nameof(Product.TagCodes), 1 },
        }));

        var aggregateFluent = collection.Aggregate<Product>(pipeline);

        return await aggregateFluent.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(int tenantCode, string productName, CancellationToken cancellationToken)
    {
        return collection
            .Find(p =>
                p!.TenantCode == tenantCode &&
                p.Name == productName).AnyAsync(cancellationToken);
    }

    public Task UpdateAsync(string oldProductName, Product product, CancellationToken cancellationToken)
    {
        return collection.ReplaceOneAsync(
            p => p!.TenantCode == product.TenantCode && p.Name == oldProductName,
            product,
            cancellationToken: cancellationToken);
    }
}