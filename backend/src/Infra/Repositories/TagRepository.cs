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

    public Task<Tag> GetAsync(TagFilter tagFilter, CancellationToken cancellationToken)
    {
        return collection
            .Find(BuildFilter(tagFilter))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task<bool> ExistsAsync(TagFilter tagFilter, CancellationToken cancellationToken)
    {
        return collection
            .Find(BuildFilter(tagFilter))
            .AnyAsync(cancellationToken);
    }

    public Task<bool> UpdateAsync(
        TagFilter tagFilter, 
        UpdateTagFilter updateFilter,
        CancellationToken cancellationToken)
    {
        var updateDefinition = BuildUpdateDefinition(updateFilter);
        
        if (updateDefinition is null)
        {
            return Task.FromResult(false);
        }
        
        return collection
            .UpdateOneAsync(BuildFilter(tagFilter), BuildUpdateDefinition(updateFilter),
                cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.ModifiedCount > 0, cancellationToken);
    }

    public Task<bool> UpdateManyAsync(
        TagFilter tagFilter,
        UpdateTagFilter updateFilter,
        CancellationToken cancellationToken)
    {
        return collection
            .UpdateManyAsync(BuildFilter(tagFilter), BuildUpdateDefinition(updateFilter),
                cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.ModifiedCount > 0, cancellationToken);
    }

    public Task<bool> DeleteAsync(int requestTenantCode, string requestTagCategoryCode, string requestTagCode,
        CancellationToken cancellationToken)
    {
        return collection
            .DeleteOneAsync(
                t => t.TenantCode == requestTenantCode &&
                     t.TagCategoryCode == requestTagCategoryCode &&
                     t.Code == requestTagCode,
                cancellationToken: cancellationToken)
            .ContinueWith(t => t.Result.DeletedCount > 0, cancellationToken);
    }

    public Task<bool> DeleteManyAsync(TagFilter tagFilter, CancellationToken cancellationToken)
    {
        return collection
            .DeleteManyAsync(BuildFilter(tagFilter), cancellationToken: cancellationToken)
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

    private static UpdateDefinition<Tag>? BuildUpdateDefinition(UpdateTagFilter updateFilter)
    {
        var updateDefinitionBuilder = Builders<Tag>.Update;
        List<UpdateDefinition<Tag>> updateOperations = [];
        
        if (updateFilter.NewTagCode is not null)
        {
            updateOperations.Add(updateDefinitionBuilder.Set(t => t.Code, updateFilter.NewTagCode));
        }

        if (updateFilter.NewTagCategoryCode is not null)
        {
            updateOperations.Add(updateDefinitionBuilder.Set(t => t.TagCategoryCode, updateFilter.NewTagCategoryCode));
        }
        
        if (updateOperations.Count == 0)
        {
            return null;
        }

        var combinedUpdateDefinition = updateDefinitionBuilder.Combine(updateOperations);

        return combinedUpdateDefinition;
    }
}