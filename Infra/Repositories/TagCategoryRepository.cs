using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infra.Repositories;

public class TagCategoryRepository(IMongoCollection<TagCategory> collection) : ITagCategoryRepository
{
    public Task CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken)
    {
        return collection.InsertOneAsync(tagCategory, cancellationToken: cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(string tagCategoryCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(tc => tc.TagCategoryCode == tagCategoryCode)
            .AnyAsync(cancellationToken);
    }
}