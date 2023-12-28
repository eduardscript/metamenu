using Core.Features.Tags.Commands;
using Core.Features.Tags.Queries;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TagMutations
{
    public async Task<IEnumerable<GetAllTags.TagDto>> CreateTag(
        [Service] IMediator mediator,
        CreateTagHandler.Command command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return await mediator.Send(new GetAllTags.Query(command.TenantCode), cancellationToken);
    }
}