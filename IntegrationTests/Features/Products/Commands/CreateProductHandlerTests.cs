using Core.Features.Products.Commands;

namespace IntegrationTests.Features.Products.Commands;

[TestClass]
public class CreateProductHandlerTests : IntegrationTestBase
{
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
            await TagRepository.CreateAsync(tag, default);
        }

        var product = Fixture.Build<Product>()
            .With(p => p.TenantCode, tenant.TenantCode)
            .With(p => p.TagCodes, tags.Select(t => t.TagCode))
            .Create();

        var handler = new CreateProductHandler.Handler(TenantRepository, TagRepository, ProductRepository);

        // Act
        await handler.Handle(
            new CreateProductHandler.Command(tenant.TenantCode, product.Name, product.Description, product.Price,
                product.TagCodes), default);

        // Assert
        var productExists = await ProductRepository.ExistsByNameAsync(product.TenantCode, product.Name, default);
        productExists.Should().BeTrue();
    }
}