using Core.Entities.Common;

namespace Core.Entities;

public record Tag(
    int TenantCode,
    string TagCategoryCode,
    string TagCode) : BaseEntity(TenantCode);