using Core.Features.TagCategories.Shared;

namespace Core.Features.TagCategories.Queries;

public static class GetAllTagCategories
{
    public record Query(int TenantCode) : IRequest<IEnumerable<TagCategoryDto>>;

    public class Handler(ITagCategoryRepository tagCategoryRepository)
        : IRequestHandler<Query, IEnumerable<TagCategoryDto>>
    {
        public async Task<IEnumerable<TagCategoryDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tags = await tagCategoryRepository.GetAllAsync(request.TenantCode, cancellationToken);

            return tags.ToDto();
        }
    }
}
