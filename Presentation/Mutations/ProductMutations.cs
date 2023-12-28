using Core.Features.Products.Commands;
using Core.Features.Products.Queries;
using Core.Repositories;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class ProductMutations
{
    public async Task<IEnumerable<GetAllProducts.ProductDto>> CreateProductAsync(
        [Service] IMediator mediator,
        CreateProductHandler.Command command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return await mediator.Send(new GetAllProducts.Query(new ProductFilter
        {
            TenantCode = command.TenantCode,
        }), cancellationToken);
    }
}