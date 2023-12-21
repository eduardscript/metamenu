namespace Core.Features.Products.Commands;

public static class CreateProduct
{
    public record Command(
        int TenantCode,
        string Name,
        string? Description,
        decimal Price,
        IEnumerable<string> TagCodes) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagRepository tagRepository,
        IProductRepository productRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var product = new Product(
                request.TenantCode,
                request.Name,
                request.Description,
                request.Price,
                request.TagCodes);

            if (!await tenantRepository.ExistsByCodeAsync(product.TenantCode, cancellationToken))
                throw new TenantNotFoundException(product.TenantCode);

            var existingTagCodes =
                await tagRepository.ExistsByCodeAsync(product.TenantCode, product.TagCodes, cancellationToken);
            if (!existingTagCodes) throw new TagNotFoundException(product.TagCodes);

            await productRepository.CreateAsync(product, cancellationToken);
        }
    }
}