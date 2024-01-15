using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;

namespace Core.Features.Products.Commands;

public static class CreateProductHandler
{
    public class Command(
        int tenantCode,
        string name,
        string? description,
        decimal price,
        IEnumerable<string> tagCodes) : IRequest
    {
        public int TenantCode { get; set; } = tenantCode;

        public string Name { get; set; } = name;

        public string? Description { get; set; } = description;

        public decimal Price { get; set; } = price;

        public IEnumerable<string> TagCodes { get; set; } = tagCodes;
    }

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

            if (!await tenantRepository.ExistsAsync(product.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(product.TenantCode);
            }

            if (!await tagRepository.ExistsAsync(product.TenantCode, product.TagCodes, cancellationToken))
            {
                throw new TagNotFoundException(product.TagCodes);
            }

            await productRepository.CreateAsync(product, cancellationToken);
        }
    }
}