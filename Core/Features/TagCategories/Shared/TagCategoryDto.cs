namespace Core.Features.TagCategories.Shared;

public record TagCategoryDto(
    int TenantCode,
    string Code);

public static class TagCategoryDtoExtensions
{
    public static IEnumerable<TagCategoryDto> ToDto(this IEnumerable<TagCategory> tagCategories)
    {
        return tagCategories.Select(tc => tc.ToDto());
    }
    
    public static TagCategoryDto ToDto(this TagCategory tagCategory)
    {
        return new TagCategoryDto(
            tagCategory.TenantCode,
            tagCategory.Code);
    }
}