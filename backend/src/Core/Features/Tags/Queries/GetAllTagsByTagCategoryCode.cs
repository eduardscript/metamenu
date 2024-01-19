using Core.Features.Tags.Shared;

namespace Core.Features.Tags.Queries;

public static class GetAllTagsByTagCategoryCode
{
    public record Query(int TenantCode, string TagCategoryCode) : IRequest<IEnumerable<TagDto>>;

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Query, IEnumerable<TagDto>>
    {
        public async Task<IEnumerable<TagDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAll(new ITagRepository.TagFilter(request.TenantCode, request.TagCategoryCode), cancellationToken);

            return tags.ToDto();
        }
    }
}