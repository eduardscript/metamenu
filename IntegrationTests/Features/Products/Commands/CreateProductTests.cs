using Core.Features.Products.Commands;

namespace IntegrationTests.Features.Products.Commands;

[TestClass]
public class CreateProductTests : IntegrationTestBase
{
    private readonly IProductRepository _productRepository = GetService<IProductRepository>();
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_CreatesProductInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

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
            .With(p => p.TagCodes, tags.Select(t => t.TagCode))
            .Create();

        var handler = new CreateProduct.Handler(_tenantRepository, _tagRepository, _productRepository);

        // Act
        await handler.Handle(
            new CreateProduct.Command(tenant.TenantCode, product.Name, product.Description, product.Price,
                product.TagCodes), default);

        // Assert
        var productExists = await _productRepository.ExistsByNameAsync(product.Name, default);
        productExists.Should().BeTrue();
    }
}