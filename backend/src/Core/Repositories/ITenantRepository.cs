using Requests = Core.Features.Tenants.Commands.ConfigureTenantCommand.Dtos.Requests;

namespace Core.Repositories;

public class UpdateTenantFilter(
    string? name = null,
    bool? isEnabled = null,
    string? defaultCategoryId = null,
    int? template = null,
    Requests. Address? address = null,
    IEnumerable<Requests.WeekDay>? weekDays = null)
{
    public string? Name { get; set; } = name;
    
    public bool? IsEnabled { get; set; } = isEnabled;

    public string? DefaultTagCategory { get; set; } = defaultCategoryId;

    public int? Template { get; set; } = template;

    public Requests.Address? Address { get; set; } = address;

    public IEnumerable<Requests.WeekDay>? WeekDays { get; set; } = weekDays;
}

public interface ITenantRepository
{
    public Task<Tenant> CreateAsync(Tenant tenant, CancellationToken cancellationToken);

    public Task<IEnumerable<Tenant>> GetAllAsync(CancellationToken cancellationToken);

    public Task<Tenant?> GetByCodeAsync(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> DeleteAsync(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> UpdateAsync(int tenantCode, UpdateTenantFilter updateTenantFilter,
        CancellationToken cancellationToken);
}