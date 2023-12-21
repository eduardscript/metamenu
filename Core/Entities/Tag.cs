using Core.Entities.Common;

namespace Core.Entities;

public record Tag(
    int TenantCode,
    string TagCode,
    string TagCategoryCode) : BaseEntity(TenantCode);