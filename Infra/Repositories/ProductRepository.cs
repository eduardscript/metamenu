using Core.Entities;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infra.Repositories;

public class ProductRepository(IMongoCollection<Product> collection) : IProductRepository
{
    public Task CreateAsync(Product product, CancellationToken cancellationToken)
    {
        return collection.InsertOneAsync(product, cancellationToken: cancellationToken);
    }

    public async Task<IEnumerable<Product>> GetAllProducts(
        ProductFilter productFilter,
        CancellationToken cancellationToken)
    {
        var pipeline = new List<BsonDocument>
        {
            new("$match", new BsonDocument("TenantCode", productFilter.TenantCode)),
        };

        if (productFilter.TagCategoryCode is not null)
        {
            pipeline.Add(new("$lookup", new BsonDocument
            {
                { "from", "Tags" },
                { "localField", "TagCodes" },
                { "foreignField", "TagCode" },
                { "as", "TagInfo" }
            }));
            pipeline.Add(new("$unwind", "$TagInfo"));
            pipeline.Add(new("$lookup", new BsonDocument
            {
                { "from", "TagCategories" },
                { "localField", "TagInfo.TagCategoryCode" },
                { "foreignField", "TagCategoryCode" },
                { "as", "TagCategoryInfo" }
            }));
            pipeline.Add(new("$unwind", "$TagCategoryInfo"));
            pipeline.Add(new("$match", new BsonDocument
            {
                { "TagCategoryInfo.TagCategoryCode", productFilter.TagCategoryCode }
            }));
        }

        pipeline.Add(new("$project", new BsonDocument
        {
            { "_id", 1 },
            { "TenantCode", 1 },
            { "Name", 1 },
            { "Description", 1 },
            { "Price", 1 },
            { "TagCodes", 1 },
        }));

        var aggregateFluent = collection.Aggregate<Product>(pipeline);

        return await aggregateFluent.ToListAsync(cancellationToken);
    }

    public Task<bool> ExistsByNameAsync(string productName, CancellationToken cancellationToken)
    {
        return collection.Find(p => p.Name == productName).AnyAsync(cancellationToken);
    }
}