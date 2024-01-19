namespace Core.Repositories;

public class UpdateTenantProperties(string? name = null, bool? isEnabled = null)
{
    public string? Name { get; set; } = name;

    public bool? IsEnabled { get; set; } = isEnabled;
}

public interface ITenantRepository
{
    public Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken);

    public Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken);
    
    public Task<Tenant?> GetAsync(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, CancellationToken cancellationToken);
    
    public Task<bool> DeleteAsync(int tenantCode, CancellationToken cancellationToken);
    
    public Task<bool> UpdateAsync(int tenantCode, UpdateTenantProperties updateTenantProperties, CancellationToken cancellationToken);
}