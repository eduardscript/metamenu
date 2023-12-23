using Core.Exceptions.Products;
using Core.Exceptions.Tenants;
using Core.Features.Products.UpdateStrategies;

namespace Core.Features.Products.Commands;

public static class UpdateProduct
{
    public class UpdateProperties
    {
        public string? Name { get; set; }
        
        public string? Description { get; set; }
        
        public decimal? Price { get; set; }
        
        public IEnumerable<string>? TagCodes { get; set; }
    }

    public class Command(
        int tenantCode,
        string name,
        UpdateProperties updateProperties) : IRequest
    {
        public int TenantCode { get; } = tenantCode;
        public string Name { get; } = name;

        public UpdateProperties UpdateProperties { get; set; } = updateProperties;
    }

    public class Handler(
        ITenantRepository tenantRepository,
        ITagRepository tagRepository,
        IProductRepository productRepository) : IRequestHandler<Command>
    {
        private readonly Dictionary<string, IUpdateProductStrategy> _strategies = new()
        {
            { nameof(Command.UpdateProperties.Name), new UpdateNameStrategy(productRepository) },
            { nameof(Command.UpdateProperties.TagCodes), new UpdateTagCodesStrategy(tagRepository) },
            { nameof(Command.UpdateProperties.Description), new UpdateDescriptionStrategy() },
            { nameof(Command.UpdateProperties.Price), new UpdatePriceStrategy() },
        };

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await tenantRepository.ExistsByAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }

            var product = await productRepository.GetByAsync(request.TenantCode, request.Name, cancellationToken);
            if (product is null)
            {
                throw new ProductNotFoundException(request.Name);
            }

            var nonNullProperties = GetNonNullProperties(request.UpdateProperties, _strategies);
            foreach (var propertyName in nonNullProperties)
            {
                if (_strategies.TryGetValue(propertyName, out var strategy))
                {
                    await strategy.UpdateProductAsync(product, request.UpdateProperties, cancellationToken);
                }
            }

            await productRepository.UpdateAsync(request.Name, product, cancellationToken);
        }
    }

    private static List<string> GetNonNullProperties(
        UpdateProperties updateProperties,
        IReadOnlyDictionary<string, IUpdateProductStrategy> strategies)
    {
        return updateProperties.GetType().GetProperties()
            .Where(p => strategies.ContainsKey(p.Name) && p.GetValue(updateProperties) is not null)
            .Select(p => p.Name)
            .ToList();
    }
}