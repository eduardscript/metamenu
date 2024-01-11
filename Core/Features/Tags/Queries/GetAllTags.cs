namespace Core.Features.Tags.Queries;

public static class GetAllTags
{
    public record Query(int TenantCode) : IRequest<IEnumerable<TagDto>>;

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Query, IEnumerable<TagDto>>
    {
        public async Task<IEnumerable<TagDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAll(new ITagRepository.TagFilter(request.TenantCode), cancellationToken);

            return tags.Select(tag => new TagDto(tag.TenantCode, tag.TagCode, tag.TagCategoryCode));
        }
    }

    public record TagDto(int TenantCode, string Code, string TagCategoryCode);
}