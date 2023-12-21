using Core.Features.TagCategories.Commands;
using Core.Features.TagCategories.Queries;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class TagCategoryMutations
{
    public async Task<IEnumerable<GetAllTagCategories.TagCategoryDto>> CreateTagCategory(
        [Service] IMediator mediator,
        CreateTagCategory.Command command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return await mediator.Send(new GetAllTagCategories.Query(command.TenantCode), cancellationToken);
    }
}