﻿using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infra.Repositories;

public class TagCategoryRepository(IMongoCollection<TagCategory> collection) : ITagCategoryRepository
{
    public Task RenameAsync(int tenantCode, string oldTagCategoryCode, string newCategoryCode, CancellationToken cancellationToken)
    {
        var filter = Builders<TagCategory>.Filter
            .Where(tc =>
                tc.TenantCode == tenantCode &&
                tc.TagCategoryCode == oldTagCategoryCode);

        var update = Builders<TagCategory>.Update.Set(tc => tc.TagCategoryCode, newCategoryCode);

        return collection.UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
    }

    public Task CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken)
    {
        return collection.InsertOneAsync(tagCategory, cancellationToken: cancellationToken);
    }

    public Task<IEnumerable<TagCategory>> GetAll(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(tc => tc.TenantCode == tenantCode)
            .ToListAsync(cancellationToken)
            .ContinueWith(tc => tc.Result.AsEnumerable(), cancellationToken);
    }

    public Task<IEnumerable<TagCategory>> GetAllTagsByTagCategoryCode(int tenantCode, string tagCategoryCode,
        CancellationToken cancellationToken)
    {
        return collection
            .Find(tc =>
                tc.TenantCode == tenantCode &&
                tc.TagCategoryCode == tagCategoryCode)
            .ToListAsync(cancellationToken)
            .ContinueWith(tc => tc.Result.AsEnumerable(), cancellationToken);
    }

    public Task<bool> ExistsByAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(tc =>
                tc.TenantCode == tenantCode &&
                tc.TagCategoryCode == tagCategoryCode)
            .AnyAsync(cancellationToken);
    }
}