using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infra.Repositories;

public class TenantRepository(IMongoCollection<Tenant> collection) : ITenantRepository
{
    public Task CreateAsync(Tenant tenant, CancellationToken cancellationToken)
    {
        return collection.InsertOneAsync(tenant, cancellationToken: cancellationToken);
    }

    public Task<bool> ExistsByCodeAsync(int tenantCode, CancellationToken cancellationToken)
    {
        return collection
            .Find(t => t.TenantCode == tenantCode)
            .AnyAsync(cancellationToken: cancellationToken);
    }
}