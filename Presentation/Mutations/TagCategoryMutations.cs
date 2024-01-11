using Core.Features.TagCategories.Commands;
using Core.Features.TagCategories.Shared;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TagCategoryMutations
{
    public async Task<TagCategoryDto> CreateTagCategory(
        [Service] IMediator mediator,
        CreateTagCategoryHandler.Command command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);

        // return await mediator.Send(new GetAllTagCategories.Query(command.TenantCode), cancellationToken);
    }
}