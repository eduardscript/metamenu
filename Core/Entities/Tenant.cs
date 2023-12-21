using Core.Entities.Common;

namespace Core.Entities;

public record Tenant(
    int TenantCode,
    string Name,
    bool IsEnabled) : BaseEntity(TenantCode);