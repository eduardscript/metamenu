using Core.Features.Tags.Shared;

namespace Core.Features.Tags.Queries;

public static class GetAllTags
{
    public record Query(int TenantCode) : IRequest<IEnumerable<TagDto>>;

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Query, IEnumerable<TagDto>>
    {
        public async Task<IEnumerable<TagDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var tags = await tagRepository.GetAllAsync(new(request.TenantCode), cancellationToken);

            return tags.ToDto();
        }
    }
}