namespace Core.Repositories;

public interface ITenantRepository
{
    public Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken);

    public Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken);

    public Task<bool> ExistsByCodeAsync(int tenantCode, CancellationToken cancellationToken);
}