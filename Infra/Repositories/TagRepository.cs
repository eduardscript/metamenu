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

    public Task<bool> ExistsByCodeAsync(IEnumerable<string> tagCodes, CancellationToken cancellationToken)
    {
        return collection
            .Find(t => tagCodes.Contains(t.Code))
            .AnyAsync(cancellationToken);
    }
    
    public Task<bool> ExistsByCodeAsync(string tagCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(t => t.Code == tagCode)
            .AnyAsync(cancellationToken);
    }
}