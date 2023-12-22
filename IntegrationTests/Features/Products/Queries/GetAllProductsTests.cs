using Core.Features.Products.Queries;

namespace IntegrationTests.Features.Products.Queries;

[TestClass]
public class GetAllProductsTests : IntegrationTestBase
{
    private static readonly Random Random = new();
    private readonly IProductRepository _productRepository = GetService<IProductRepository>();
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_ReturnsAllProductsForTenant()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();
        await _tenantRepository.CreateAsync(tenant, default);

        var tags = Fixture
            .Build<Tag>()
            .With(t => t.TenantCode, tenant.TenantCode)
            .CreateMany()
            .ToList();

        foreach (var tag in tags) await _tagRepository.CreateAsync(tag, default);

        var expectedProducts = Fixture.Build<Product>()
            .With(p => p.TenantCode, tenant.TenantCode)
            .With(p => p.TagCodes, () => tags.Take(Random.Next(tags.Count)).Select(t => t.TagCode))
            .CreateMany()
            .ToList();

        foreach (var product in expectedProducts) await _productRepository.CreateAsync(product, default);

        var handler = new GetAllProducts.Handler(_productRepository);

        // Act
        var result = await handler.Handle(new GetAllProducts.Query(new ProductFilter
        {
            TenantCode = tenant.TenantCode
        }), default);

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(expectedProducts.Count);
        foreach (var expectedProduct in expectedProducts)
            resultList.Should().ContainEquivalentOf(new GetAllProducts.ProductDto(
                expectedProduct.TenantCode,
                expectedProduct.Name,
                expectedProduct.Description,
                expectedProduct.Price,
                expectedProduct.TagCodes));
    }
}