using Core.Features.TagCategories.Queries;

namespace IntegrationTests.Features.TagCategories.Queries;

[TestClass]
public class GetAllTagCategoriesTests : IntegrationTestBase
{
    private readonly ITagCategoryRepository _tagCategoryRepository;

    public GetAllTagCategoriesTests()
    {
        _tagCategoryRepository = GetService<ITagCategoryRepository>();
    }

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
            await _tagCategoryRepository.CreateAsync(tagCategory, default);

        var handler = new GetAllTagCategories.Handler(_tagCategoryRepository);

        // Act
        var result = await handler.Handle(new GetAllTagCategories.Query(tenantCode), default);

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(expectedTagCategories.Count);
        foreach (var expectedTagCategory in expectedTagCategories)
            resultList.Should()
                .ContainEquivalentOf(
                    new GetAllTagCategories.TagCategoryDto(tenantCode, expectedTagCategory.TagCategoryCode));
    }
}