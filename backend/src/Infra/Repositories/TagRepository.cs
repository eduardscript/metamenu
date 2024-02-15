using Core.Repositories;
using MongoDB.Driver;
using Tag = Core.Entities.Tag;

namespace Infra.Repositories;

public class TagRepository(IMongoCollection<Tag> collection) : ITagRepository
{
    public async Task<Tag> CreateAsync(Tag tag, CancellationToken cancellationToken)
    {
        await collection.InsertOneAsync(tag, cancellationToken: cancellationToken);

        return tag;
    }

    public Task<IEnumerable<Tag>> GetAllAsync(TagFilter tagFilter, CancellationToken cancellationToken)
    {
        return collection
            .Find(BuildFilter(tagFilter))
            .ToListAsync(cancellationToken)
            .ContinueWith(tags => tags.Result.AsEnumerable(), cancellationToken);
    }

    public Task<bool> ExistsAsync(int tenantCode, IEnumerable<string> tagCodes,
        CancellationToken cancellationToken)
    {
        return collection
            .Find(t =>
                t.TenantCode == tenantCode &&
                tagCodes.Contains(t.Code))
            .AnyAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(TagFilter tagFilter, CancellationToken cancellationToken)
    {
        return collection
            .Find(BuildFilter(tagFilter))
            .AnyAsync(cancellationToken);
    }

    public Task RenameAsync(int tenantCode, string oldTagCode, string newTagCode, CancellationToken cancellationToken)
    {
        return collection
            .UpdateOneAsync(
                t => t.TenantCode == tenantCode &&
                     t.Code == oldTagCode,
                Builders<Tag>.Update.Set(t => t.Code, newTagCode),
                cancellationToken: cancellationToken);
    }
    
    public Task<bool> DeleteAsync(int requestTenantCode, string requestTagCategoryCode, string requestTagCode, CancellationToken cancellationToken)
    {
        return collection
            .DeleteOneAsync(
                t => t.TenantCode == requestTenantCode &&
                     t.TagCategoryCode == requestTagCategoryCode &&
                     t.Code == requestTagCode,
                cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.DeletedCount > 0, cancellationToken);
    }

    private static FilterDefinition<Tag> BuildFilter(TagFilter tagFilter)
    {
        var filter = Builders<Tag>.Filter.Eq(t => t.TenantCode, tagFilter.TenantCode);

        if (tagFilter.TagCategoryCode is not null)
        {
            filter &= Builders<Tag>.Filter.Eq(t => t.TagCategoryCode, tagFilter.TagCategoryCode);
        }
        
        if (tagFilter.Code is not null)
        {
            filter &= Builders<Tag>.Filter.Eq(t => t.Code, tagFilter.Code);
        }
        
        if (tagFilter.Codes is not null)
        {
            filter &= Builders<Tag>.Filter.In(t => t.Code, tagFilter.Codes);
        }
        
        return filter;
    }
}