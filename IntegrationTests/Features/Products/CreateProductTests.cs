using Core.Features.Products;

namespace IntegrationTests.Features.Products;

[Trait(nameof(Constants.Features), Constants.Features.Products)]
public class CreateProductTests : IntegrationTestBase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IProductRepository _productRepository;

    public CreateProductTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
        _tagRepository = GetService<ITagRepository>();
        _productRepository = GetService<IProductRepository>();
    }

    [Fact]
    public async Task Handle_CreatesProductInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();
        await _tenantRepository.CreateAsync(tenant, default);

        var tags = Fixture.Build<Tag>()
            .With(t => t.TenantCode, tenant.TenantCode)
            .CreateMany()
            .ToList();

        foreach (var tag in tags)
        {
            await _tagRepository.CreateAsync(tag, default);
        }

        var product = Fixture.Build<Product>()
            .With(p => p.TenantCode, tenant.TenantCode)
            .With(p => p.TagCodes, tags.Select(t => t.Code))
            .Create();

        var handler = new CreateProduct.Handler(_tenantRepository, _tagRepository, _productRepository);

        // Act
        await handler.Handle(new CreateProduct.Command(tenant.TenantCode, product.Name, product.Description, product.Price, product.TagCodes), default);

        // Assert
        var productExists = await _productRepository.ExistsByNameAsync(product.Name, default);
        Assert.True(productExists);
    }
}
