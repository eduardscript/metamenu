using Core.Features.Tags.Commands;
using Core.Features.Tags.Queries;
using Core.Features.Tags.Shared;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TagMutations
{
    public async Task<IEnumerable<TagDto>> CreateTag(
        [Service] IMediator mediator,
        CreateTag.Command command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return await mediator.Send(new GetAllTags.Query(command.TenantCode), cancellationToken);
    }
    
    public Task<DeleteTag.TagDeletedDto> DeleteTag(
        [Service] IMediator mediator,
        DeleteTag.Command command,
        CancellationToken cancellationToken)
    {
        return mediator.Send(command, cancellationToken);
    }
}