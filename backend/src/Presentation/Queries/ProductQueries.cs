using Core.Features.Products.Queries;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class ProductQueries
{
    public Task<IEnumerable<GetAllProducts.ProductDto>> GetAllProductsAsync(
        [Service] IMediator mediator,
        GetAllProducts.Query query,
        CancellationToken cancellationToken)
    {
        return mediator.Send(query, cancellationToken);
    }
}