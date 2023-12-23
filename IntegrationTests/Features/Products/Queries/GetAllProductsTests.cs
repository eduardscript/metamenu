using Core.Features.Products.Queries;

namespace IntegrationTests.Features.Products.Queries;

[TestClass]
public class GetAllProductsTests : IntegrationTestBase
{
    private static readonly Random Random = new();
    private readonly IProductRepository _productRepository = GetService<IProductRepository>();
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    private Tenant _tenant = default!;
    private List<Tag> _tags = default!;
    private List<Product> _expectedProducts = default!;
    private GetAllProducts.Handler _handler = default!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _tenant = await MongoDbFixture.CreateTenantAsync();
        _tags = Fixture
            .Build<Tag>()
            .With(t => t.TenantCode, _tenant.TenantCode)
            .CreateMany()
            .ToList();

        foreach (var tag in _tags) await _tagRepository.CreateAsync(tag, default);
        
        _expectedProducts = Fixture.Build<Product>()
            .With(p => p.TenantCode, _tenant.TenantCode)
            .With(p => p.TagCodes, () => _tags.Take(Random.Next(_tags.Count)).Select(t => t.TagCode))
            .CreateMany()
            .ToList();

        foreach (var product in _expectedProducts) await _productRepository.CreateAsync(product, default);

        _handler = new GetAllProducts.Handler(_productRepository);

    }

    [TestMethod]
    public async Task Handle_ReturnsAllProductsForTenant()
    {
        // Act
        var result = await _handler.Handle(
            new GetAllProducts.Query(new ProductFilter
            {
                TenantCode = _tenant.TenantCode
            }), default);

        // Assert
        AssertProducts(result, _expectedProducts);
    }
    
    [TestMethod]
    public async Task Handle_ReturnsAllProductsForTenantAndTagCodes()
    {
        // Arrange
        var randomTag = _tags.Take(Random.Next(_tags.Count)).ToList();

        // Act
        var result = await _handler.Handle(
            new GetAllProducts.Query(new ProductFilter
            {
                TenantCode = _tenant.TenantCode,
                TagCodes = randomTag.Select(t => t.TagCode).ToList()
            }), default);

        // Assert
        var productsWithRandomTag = _expectedProducts.Where(p => p.TagCodes.Intersect(randomTag.Select(t => t.TagCode)).Any()).ToList();
        AssertProducts(result, productsWithRandomTag);
    }

    private static void AssertProducts(IEnumerable<GetAllProducts.ProductDto> result, List<Product> expectedProducts)
    {
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