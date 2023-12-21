namespace Core.Features.TagCategories.Queries;

public static class GetAllTagCategories
{
    public record Query(int TenantCode) : IRequest<IEnumerable<TagCategoryDto>>;

    public class Handler(ITagCategoryRepository tagCategoryRepository)
        : IRequestHandler<Query, IEnumerable<TagCategoryDto>>
    {
        public async Task<IEnumerable<TagCategoryDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tags = await tagCategoryRepository.GetAllTags(request.TenantCode, cancellationToken);

            return tags.Select(tag => new TagCategoryDto(tag.TenantCode, tag.TagCategoryCode));
        }
    }

    public record TagCategoryDto(int TenantCode, string TagCategoryCode);
}