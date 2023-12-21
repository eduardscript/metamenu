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

    public Task<IEnumerable<Tag>> GetAllTags(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(t => t.TenantCode == tenantCode)
            .ToListAsync(cancellationToken)
            .ContinueWith(tags => tags.Result.AsEnumerable(), cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(int tenantCode, IEnumerable<string> tagCodes,
        CancellationToken cancellationToken)
    {
        return collection
            .Find(t =>
                t.TenantCode == tenantCode &&
                tagCodes.Contains(t.TagCode))
            .AnyAsync(cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(int tenantCode, string tagCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(
                t => t.TenantCode == tenantCode &&
                     t.TagCode == tagCode)
            .AnyAsync(cancellationToken);
    }
}