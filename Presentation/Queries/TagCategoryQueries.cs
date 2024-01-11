using Core.Features.TagCategories.Queries;
using Core.Features.TagCategories.Shared;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TagCategoryQueries
{
    public Task<IEnumerable<TagCategoryDto>> GetAllTagCategoriesAsync(
        [Service] IMediator mediator,
        GetAllTagCategories.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}