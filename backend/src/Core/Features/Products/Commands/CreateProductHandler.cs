using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Products.Shared;

namespace Core.Features.Products.Commands;

public static class CreateProductHandler
{
    public class Command(
        int tenantCode,
        string name,
        string? description,
        decimal price,
        IEnumerable<string> tagCodes) : IRequest<ProductDto>
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
        IProductRepository productRepository) : IRequestHandler<Command, ProductDto>
    {
        public async Task<ProductDto> Handle(Command request, CancellationToken cancellationToken)
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

            var existingTags =
                await tagRepository.GetAll(new ITagRepository.TagFilter(product.TenantCode), cancellationToken);

            var invalidTagCodes = product.TagCodes.Except(existingTags.Select(t => t.Code)).ToList();

            if (invalidTagCodes.Any())
            {
                throw new TagNotFoundException(invalidTagCodes);
            }

            var newProduct = await productRepository.CreateAsync(product, cancellationToken);

            return newProduct.ToDto();
        }
    }
}