using Core.Repositories;
using MongoDB.Driver;
using Tag = Core.Entities.Tag;

namespace Infra.Repositories;

public class TagRepository(IMongoCollection<Tag> collection) : ITagRepository
{
    public Task CreateAsync(Tag tag, CancellationToken cancellationToken)
    {
        return collection.InsertOneAsync(tag, cancellationToken: cancellationToken);
    }

    public Task<IEnumerable<Tag>> GetAll(ITagRepository.TagFilter tagFilter, CancellationToken cancellationToken)
    {
        var filter = Builders<Tag>.Filter.Eq(t => t.TenantCode, tagFilter.TenantCode);

        if (tagFilter.TagCategoryCode is not null)
        {
            filter &= Builders<Tag>.Filter.Eq(t => t.TagCategoryCode, tagFilter.TagCategoryCode);
        }

        return collection
            .Find(filter)
            .ToListAsync(cancellationToken)
            .ContinueWith(tags => tags.Result.AsEnumerable(), cancellationToken);
    }

    public Task<bool> ExistsAsync(int tenantCode, IEnumerable<string> tagCodes,
        CancellationToken cancellationToken)
    {
        return collection
            .Find(t =>
                t.TenantCode == tenantCode &&
                tagCodes.Contains(t.TagCode))
            .AnyAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(int tenantCode, string tagCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(
                t => t.TenantCode == tenantCode &&
                     t.TagCode == tagCode)
            .AnyAsync(cancellationToken);
    }

    public Task RenameAsync(int tenantCode, string oldTagCode, string newTagCode, CancellationToken cancellationToken)
    {
        return collection
            .UpdateOneAsync(
                t => t.TenantCode == tenantCode &&
                     t.TagCode == oldTagCode,
                Builders<Tag>.Update.Set(t => t.TagCode, newTagCode),
                cancellationToken: cancellationToken);
    }
}