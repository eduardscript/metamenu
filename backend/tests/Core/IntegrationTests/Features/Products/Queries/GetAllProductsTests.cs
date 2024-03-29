﻿using Core.Features.Products.Queries;
using Core.Features.Products.Shared;

namespace IntegrationTests.Features.Products.Queries;

[TestClass]
public class GetAllProductsTests : BaseIntegrationTest
{
    private static readonly Random Random = new();

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
            .With(t => t.TenantCode, _tenant.Code)
            .CreateMany()
            .ToList();

        foreach (var tag in _tags) await TagRepository.CreateAsync(tag, default);

        _expectedProducts = Fixture.Build<Product>()
            .With(p => p.TenantCode, _tenant.Code)
            .With(p => p.TagCodes, () => _tags.Take(Random.Next(_tags.Count)).Select(t => t.Code))
            .CreateMany()
            .ToList();

        foreach (var product in _expectedProducts) await ProductRepository.CreateAsync(product, default);

        _handler = new GetAllProducts.Handler(ProductRepository);
    }

    [TestMethod]
    public async Task Handle_ReturnsAllProductsForTenant()
    {
        // Act
        var result = await _handler.Handle(
            new GetAllProducts.Query(new ProductFilter(_tenant.Code)), default);

        // Assert
        AssertProducts(result, _expectedProducts);
    }

    [TestMethod]
    public async Task Handle_ReturnsAllProductsForTenantAndTagCodes()
    {
        // Arrange
        var randomTagCodes = _tags.Take(2).Select(t => t.Code).ToList();

        // Act
        var result = await _handler.Handle(
            new GetAllProducts.Query(new ProductFilter(_tenant.Code)
            {
                TagCodes = randomTagCodes
            }), default);

        // Assert
        var productsWithRandomTag = _expectedProducts.Where(p => p.TagCodes.Intersect(randomTagCodes).Any()).ToList();
        AssertProducts(result, productsWithRandomTag);
    }

    private static void AssertProducts(IEnumerable<ProductDto> result, List<Product> expectedProducts)
    {
        var resultList = result.ToList();
        resultList.Count.Should().BeGreaterThan(0);
        expectedProducts.Count.Should().BeGreaterThan(0);

        resultList.Should().HaveCount(expectedProducts.Count);
        foreach (var expectedProduct in expectedProducts)
            resultList.Should().ContainEquivalentOf(new ProductDto(
                expectedProduct.Code,
                expectedProduct.TenantCode,
                expectedProduct.Name,
                expectedProduct.Description,
                expectedProduct.Price,
                expectedProduct.TagCodes));
    }
}