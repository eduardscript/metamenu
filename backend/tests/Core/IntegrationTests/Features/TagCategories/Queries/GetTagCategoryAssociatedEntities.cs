using Core.Features.TagCategories.Queries;

namespace IntegrationTests.Features.TagCategories.Queries;

[TestClass]
public class GetTagCategoryAssociatedEntitiesTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_ReturnsTagCategoryAssociatedEntitiesForTenant()
    {
        // Arrange
        var tenantCode = (await MongoDbFixture.CreateTenantAsync()).Code;
        
        var tagCategory = Fixture.Build<TagCategory>()
            .With(t => t.TenantCode, tenantCode)
            .Create();
        
        await TagCategoryRepository.CreateAsync(tagCategory, default);

        var tags = Fixture.Build<Tag>()
            .With(t => t.TenantCode, tenantCode)
            .With(t => t.TagCategoryCode, tagCategory.Code)
            .CreateMany(10).ToArray();

        foreach (var tag in tags)
        {
            await TagRepository.CreateAsync(tag, default);
        }
        
        var products = Fixture.Build<Product>()
            .With(p => p.TenantCode, tenantCode)
            .With(p => p.TagCodes, tags.GetRandom(5).Select(t => t.Code).ToArray())
            .CreateMany(10)
            .ToArray();

        foreach (var product in products)
        {
            await ProductRepository.CreateAsync(product, default);
        }
        
        // Act
        var handler = new GetTagCategoryAssociatedEntities.Handler(
            TagRepository,
            ProductRepository);

        var query = new GetTagCategoryAssociatedEntities.Query(tenantCode, tagCategory.Code);

        var result = (await handler.Handle(query, default))
            .ToList();
        
        // Assert
        result.Should().NotBeEmpty();
        result.Should().HaveCount(products.Length);
        
        foreach (var dto in result)
        {
            dto.Tag.Should().NotBeNull();

            tags.Select(t => t.Code).Should().Contain(dto.Tag);

            var expectedProducts = products.Where(p => p.TagCodes.Contains(dto.Tag));

            dto.Products.Should().BeEquivalentTo(expectedProducts, options => options.WithStrictOrdering());
        }
    }
}

