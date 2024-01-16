using Core.Features.Products.Commands;
using Core.Features.Products.Shared;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class ProductMutations
{
    public async Task<ProductDto> CreateProductAsync(
        [Service] IMediator mediator,
        CreateProductHandler.Command command,
        CancellationToken cancellationToken)
    {
        return await mediator.Send(command, cancellationToken);
    }
}