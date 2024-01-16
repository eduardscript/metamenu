namespace Core.Repositories;

public interface ITenantRepository
{
    public Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken);

    public Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<Tenant?> GetAsync(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, CancellationToken cancellationToken);
    
    public Task<bool> DeleteAsync(int tenantCode, CancellationToken cancellationToken);
    
    public Task<bool> Update(int tenantCode, bool status, CancellationToken cancellationToken);
}