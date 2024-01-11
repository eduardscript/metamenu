namespace Core.Features.Tags.Shared;

public record TagDto(
    int TenantCode,
    string TagCategoryCode,
    string TagCode);

public static class TagsDtoExtensions
{
    public static TagDto ToDto(this TagDto tag)
    {
        return new TagDto(
            tag.TenantCode,
            tag.TagCategoryCode,
            tag.TagCode);
    }
}