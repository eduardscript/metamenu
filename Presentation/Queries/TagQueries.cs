using Core.Features.Tags.Queries;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TagQueries
{
    public Task<IEnumerable<GetAllTags.TagDto>> GetAllTagsAsync(
        [Service] IMediator mediator,
        GetAllTags.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}