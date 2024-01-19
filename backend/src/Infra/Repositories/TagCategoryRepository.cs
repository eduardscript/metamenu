using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infra.Repositories;

public class TagCategoryRepository(IMongoCollection<TagCategory> collection) : ITagCategoryRepository
{
    public Task RenameAsync(int tenantCode, string oldTagCategoryCode, string newCategoryCode,
        CancellationToken cancellationToken)
    {
        return collection
            .UpdateOneAsync(
                tc => tc.TenantCode == tenantCode &&
                      tc.Code == oldTagCategoryCode,
                Builders<TagCategory>.Update.Set(t => t.Code, newCategoryCode),
                cancellationToken: cancellationToken);
    }

    public async Task<TagCategory> CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken)
    {
        await collection.InsertOneAsync(tagCategory, cancellationToken: cancellationToken);
        
        return tagCategory;
    }

    public Task<IEnumerable<TagCategory>> GetAllAsync(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(tc => tc.TenantCode == tenantCode)
            .ToListAsync(cancellationToken)
            .ContinueWith(tc => tc.Result.AsEnumerable(), cancellationToken);
    }

    public async Task<TagCategory?> GetByAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken)
    {
        return await collection
            .Find(tc =>
                tc.TenantCode == tenantCode &&
                tc.Code == tagCategoryCode)
            .FirstOrDefaultAsync(cancellationToken);
    }


    public Task<bool> ExistsAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(tc =>
                tc.TenantCode == tenantCode &&
                tc.Code == tagCategoryCode)
            .AnyAsync(cancellationToken);
    }
}