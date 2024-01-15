using Core.Features.Products.Queries;
using Core.Features.Products.Shared;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class ProductQueries
{
    public Task<IEnumerable<ProductDto>> GetAllProductsAsync(
        [Service] IMediator mediator,
        GetAllProducts.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}