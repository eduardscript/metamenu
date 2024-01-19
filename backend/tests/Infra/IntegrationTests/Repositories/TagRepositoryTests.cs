using Core.Repositories;

namespace IntegrationTests.Repositories;

[TestClass]
public class TagRepositoryTests : BaseIntegrationTest
{
    #region CreateAsync

    [TestMethod]
    public async Task CreateAsync_Creates()
    {
        // Arrange
        var tagToInsert = Fixture.Create<Tag>();

        // Act
        var tagFromDb = await TagRepository.CreateAsync(tagToInsert, default);

        // Assert
        tagFromDb.Should().BeEquivalentTo(tagToInsert);
    }

    #endregion
    
    #region GetAllAsync
    
    [TestMethod]
    public async Task GetAllAsync_ByTenantCodeAndCode_Gets()
    {
        // Arrange
        var tagCategoryCodes = Fixture.CreateMany<string>(3).ToList();
        
        var tagsToInsert = Fixture
            .Build<Tag>()
            .With(t => t.TenantCode, Enumerable.Range(1, 2).TakeFirstRandom())
            .With(t => t.TagCategoryCode, tagCategoryCodes.TakeFirstRandom())
            .CreateMany(6)
            .ToList();

        foreach (var tagToInsert in tagsToInsert)
        {
            await TagRepository.CreateAsync(tagToInsert, default);
        }

        var tenantCode = tagsToInsert.First().TenantCode;
        var tagCategoryCode = tagsToInsert.First(t => t.TenantCode == tenantCode).TagCategoryCode;

        var filteredTags = tagsToInsert
            .Where(t => t.TenantCode == tenantCode && t.TagCategoryCode == tagCategoryCode)
            .ToList();

        // Act
        var filter = new TagFilter(tenantCode, tagCategoryCode);
        var existingTags = await TagRepository.GetAllAsync(filter, default);

        // Assert
        existingTags.Should().BeEquivalentTo(filteredTags);
    }
    
    #endregion
}