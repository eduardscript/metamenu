using Core.Features.Tags.Queries;
using Core.Features.Tags.Shared;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TagQueries
{
    public Task<IEnumerable<TagDto>> GetAllTagsAsync(
        [Service] IMediator mediator,
        GetAllTags.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}