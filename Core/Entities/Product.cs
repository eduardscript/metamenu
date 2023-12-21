using Core.Entities.Common;

namespace Core.Entities;

public record Product(
    int TenantCode,
    string Name,
    string? Description,
    decimal Price,
    IEnumerable<string> TagCodes) : BaseEntity(TenantCode);