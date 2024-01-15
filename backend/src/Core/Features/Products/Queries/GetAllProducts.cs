using Core.Features.Products.Shared;

namespace Core.Features.Products.Queries;

public static class GetAllProducts
{
    public record Query(ProductFilter ProductFilter) : IRequest<IEnumerable<ProductDto>>;

    public class Handler(
        IProductRepository productRepository) : IRequestHandler<Query, IEnumerable<ProductDto>>
    {
        public async Task<IEnumerable<ProductDto>> Handle(Query request, CancellationToken cancellationToken)
        {
            var products = await productRepository.GetAllAsync(
                request.ProductFilter,
                cancellationToken);

            return products.Select(p => p.ToDto());
        }
    }
}