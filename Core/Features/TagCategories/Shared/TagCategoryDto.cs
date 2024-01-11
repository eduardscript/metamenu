namespace Core.Features.TagCategories.Shared;

public record TagCategoryDto(
    int TenantCode,
    string Code);

public static class TagCategoryDtoExtensions
{
    public static TagCategoryDto ToDto(this TagCategory tagCategory)
    {
        return new TagCategoryDto(
            tagCategory.TenantCode,
            tagCategory.Code);
    }
}