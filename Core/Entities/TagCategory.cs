using Core.Entities.Common;

namespace Core.Entities;

public record TagCategory(
    int TenantCode,
    string TagCategoryCode) : BaseEntity(TenantCode);