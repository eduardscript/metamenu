namespace Core.Features.Products;

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
            if (!await tenantRepository.ExistsByCodeAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }

            var existingTagCodes = await tagRepository.ExistsByCodeAsync(request.TagCodes, cancellationToken);
            if (!existingTagCodes)
            {
                throw new TagNotFoundException(request.TagCodes);
            }

            var product = new Product(
                request.TenantCode,
                request.Name,
                request.Description,
                request.Price,
                request.TagCodes);

            await productRepository.CreateAsync(product, cancellationToken);
        }
    }
}