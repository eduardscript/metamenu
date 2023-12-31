﻿using Core.Entities;
using Core.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infra.Repositories;

public class TenantRepository(IMongoCollection<Tenant> collection) : ITenantRepository
{
    public async Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        var aggregateFluent = collection.Aggregate()
            .Group(new BsonDocument 
            { 
                { "_id", BsonNull.Value },
                { "TenantCode", new BsonDocument("$max", "$TenantCode") }
            })
            .Project(new BsonDocument
            {
                { "_id", 0 },
                { "TenantCode", 1 }
            });
        
        var tenantCode = await aggregateFluent.FirstOrDefaultAsync(cancellationToken);

        tenant = tenant with { TenantCode = tenantCode is null ? 1 : + 1 };

        await collection.InsertOneAsync(tenant, cancellationToken: cancellationToken);
        
        return tenant;
    }

    public Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken)
    {
        return collection
            .Find(FilterDefinition<Tenant>.Empty)
            .ToListAsync(cancellationToken)
            .ContinueWith(t => t.Result.AsEnumerable(), cancellationToken);
    }

    public Task<bool> ExistsAsync(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(t => t.TenantCode == tenantCode)
            .AnyAsync(cancellationToken);
    }
}