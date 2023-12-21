using Core.Features.TagCategories.Queries;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class TagCategoryQueries
{
    public Task<IEnumerable<GetAllTagCategories.TagCategoryDto>> GetAllTagCategoriesAsync(
        [Service] IMediator mediator,
        GetAllTagCategories.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}