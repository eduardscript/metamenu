using Core.Entities;
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
                { nameof(Tenant.Code), new BsonDocument("$max", "$Code") }
            })
            .Project<Tenant>(new BsonDocument
            {
                { "_id", 0 },
                { nameof(Tenant.Code), 1 }
            });

        var tenantCode = (await aggregateFluent.FirstOrDefaultAsync(cancellationToken))?.Code;

        tenant.Code = tenantCode is null ? 1000 : tenantCode.Value + 1000;

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

    public async Task<Tenant?> GetAsync(int tenantCode, CancellationToken cancellationToken)
    {
        var existingTenant = await collection
            .Find(t => t.Code == tenantCode)
            .FirstOrDefaultAsync(cancellationToken);
        
        return existingTenant;
    }

    public Task<bool> ExistsAsync(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(t => t.Code == tenantCode)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> UpdateAsync(int tenantCode, UpdateTenantProperties updateTenantProperties, CancellationToken cancellationToken)
    {
        var updateDefinition = Builders<Tenant>.Update;
        var updates = new List<UpdateDefinition<Tenant>>();

        if (updateTenantProperties.Name is not null)
        {
            updates.Add(updateDefinition.Set(t => t.Name, updateTenantProperties.Name));
        }
        
        if (updateTenantProperties.IsEnabled is not null)
        {
            updates.Add(updateDefinition.Set(t => t.IsEnabled, updateTenantProperties.IsEnabled));
        }
        
        var result = await collection.UpdateOneAsync(
            t => t.Code == tenantCode,
            updateDefinition.Combine(updates),
            cancellationToken: cancellationToken);
        
        return result.ModifiedCount > 0;
    }
    
    public Task<bool> DeleteAsync(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .DeleteOneAsync(t => t.Code == tenantCode, cancellationToken)
            .ContinueWith(t => t.Result.DeletedCount > 0, cancellationToken);
    }
}