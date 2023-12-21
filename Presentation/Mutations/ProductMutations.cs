using Core.Features.Products.Commands;
using Core.Features.Products.Queries;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class ProductMutations
{
    public async Task<IEnumerable<GetAllProducts.ProductDto>> CreateProductAsync(
        [Service] IMediator mediator,
        CreateProduct.Command command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return await mediator.Send(new GetAllProducts.Query(command.TenantCode), cancellationToken);
    }
}