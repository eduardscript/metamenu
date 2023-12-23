using Core.Features.Products.Commands;

namespace IntegrationTests.Features.Products.Commands;

[TestClass]
public class UpdateProductTests : IntegrationTestBase
{
    private readonly IProductRepository _productRepository = GetService<IProductRepository>();
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_FullUpdate()
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
        
        await _productRepository.CreateAsync(product, default);

        var handler = new UpdateProduct.Handler(_tenantRepository, _tagRepository, _productRepository);

        var updateProperties = Fixture.Build<UpdateProduct.UpdateProperties>()
            .With(p => p.TagCodes, tags.Take(2).Select(t => t.TagCode))
            .Create();
        
        // Act
        await handler.Handle(
            new UpdateProduct.Command(tenant.TenantCode, product.Name, updateProperties), 
            default);

        // Assert
        var updatedProduct = await _productRepository.GetByAsync(product.TenantCode, updateProperties.Name!, default);
        updatedProduct.Should().BeEquivalentTo(updateProperties);
        
        var oldProduct = await _productRepository.ExistsByNameAsync(product.TenantCode, product.Name, default);
        oldProduct.Should().BeFalse();
    }
}