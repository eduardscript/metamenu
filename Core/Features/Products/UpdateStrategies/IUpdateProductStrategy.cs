using Core.Exceptions.Products;
using Core.Exceptions.Tags;
using Core.Features.Products.Commands;

namespace Core.Features.Products.UpdateStrategies;

public interface IUpdateProductStrategy
{
    Task UpdateProductAsync(Product product, UpdateProduct.UpdateProperties command, CancellationToken cancellationToken);
}

public class UpdateNameStrategy(IProductRepository productRepository) : IUpdateProductStrategy
{
    public async Task UpdateProductAsync(Product product, UpdateProduct.UpdateProperties command, CancellationToken cancellationToken)
    {
        var existingProductName = await productRepository.ExistsByNameAsync(product.TenantCode, command.Name!, cancellationToken);
        if (existingProductName)
        {
            throw new ProductAlreadyExistsException(command.Name!);
        }
        
        product.Name = command.Name!;
    }
}


public class UpdatePriceStrategy : IUpdateProductStrategy
{
    public Task UpdateProductAsync(Product product, UpdateProduct.UpdateProperties command, CancellationToken cancellationToken)
    {
        product.Price = command.Price!.Value;

        return Task.CompletedTask;
    }
}

public class UpdateDescriptionStrategy : IUpdateProductStrategy
{
    public Task UpdateProductAsync(Product product, UpdateProduct.UpdateProperties command, CancellationToken cancellationToken)
    {
        product.Description = command.Description;

        return Task.FromResult(product);
    }
}

public class UpdateTagCodesStrategy(ITagRepository tagRepository) : IUpdateProductStrategy
{
    public async Task UpdateProductAsync(Product product, UpdateProduct.UpdateProperties command,
        CancellationToken cancellationToken)
    {
        var existingTagCodes =
            await tagRepository.ExistsAsync(product.TenantCode, command.TagCodes!, cancellationToken);

        if (!existingTagCodes)
        {
            throw new TagNotFoundException(command.TagCodes!);
        }

        product.TagCodes = command.TagCodes!;
    }
}
