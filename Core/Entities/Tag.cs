using Core.Entities.Common;

namespace Core.Entities;

public record Tag(
    int TenantCode,
    string Code,
    string TagCategoryCode) : BaseEntity(TenantCode);