namespace Core.Features.Tags.Shared;

public record TagDto(
    int TenantCode,
    string TagCategoryCode,
    string TagCode);

public static class TagsDtoExtensions
{
    public static IEnumerable<TagDto> ToDto(this IEnumerable<Tag> tags)
    {
        return tags.Select(t => t.ToDto());
    }
    
    public static TagDto ToDto(this Tag tag)
    {
        return new TagDto(
            tag.TenantCode,
            tag.TagCategoryCode,
            tag.TagCode);
    }
}