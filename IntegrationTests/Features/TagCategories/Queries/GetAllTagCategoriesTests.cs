using Core.Features.TagCategories.Queries;
using Core.Features.TagCategories.Shared;

namespace IntegrationTests.Features.TagCategories.Queries;

[TestClass]
public class GetAllTagCategoriesTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_ReturnsAllTagCategoriesForTenant()
    {
        // Arrange
        var tenantCode = Fixture.Create<int>();
        var expectedTagCategories = Fixture.Build<TagCategory>()
            .With(tc => tc.TenantCode, tenantCode)
            .CreateMany()
            .ToList();

        foreach (var tagCategory in expectedTagCategories)
            await TagCategoryRepository.CreateAsync(tagCategory, default);

        var handler = new GetAllTagCategories.Handler(TagCategoryRepository);

        // Act
        var result = await handler.Handle(new GetAllTagCategories.Query(tenantCode), default);

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(expectedTagCategories.Count);
        foreach (var expectedTagCategory in expectedTagCategories)
            resultList.Should()
                .ContainEquivalentOf(
                    new TagCategoryDto(tenantCode, expectedTagCategory.Code));
    }
}