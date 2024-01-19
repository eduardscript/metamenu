namespace IntegrationTests.Repositories;

[TestClass]
public class TagCategoryRepositoryTests : BaseIntegrationTest
{
    #region CreateAsync

    [TestMethod]
    public async Task CreateAsync_Creates()
    {
        // Arrange
        var tagCategoryToInsert = Fixture.Create<TagCategory>();

        // Act
        var tagCategoryFromDb = await TagCategoryRepository.CreateAsync(tagCategoryToInsert, default);

        // Assert
        tagCategoryFromDb.Should().BeEquivalentTo(tagCategoryToInsert);
    }

    #endregion
    
    #region GetAllAsync

    [TestMethod]
    public async Task GetAllAsync_ByTenantCode_Gets()
    {
        // Arrange
        var tagCategoriesToInsert = Fixture
            .Build<TagCategory>()
            .With(tc => tc.TenantCode, Enumerable.Range(1, 2).TakeFirstRandom())
            .CreateMany(6)
            .ToList();

        foreach (var tagCategoryToInsert in tagCategoriesToInsert)
        {
            await TagCategoryRepository.CreateAsync(tagCategoryToInsert, default);
        }

        var tenantCode = tagCategoriesToInsert.First().TenantCode;
        var filteredTagCategories = tagCategoriesToInsert
            .Where(tc => tc.TenantCode == tenantCode)
            .ToList();

        // Act
        var existingTagCategories = await TagCategoryRepository.GetAllAsync(tenantCode,default);

        // Assert
        existingTagCategories.Should().BeEquivalentTo(filteredTagCategories);
    }

    #endregion
    
    #region GetByAsync
    
    [TestMethod]
    public async Task GetByAsync_ByTenantCodeAndTagCategoryCode_Gets()
    {
        // Arrange
        var insertedTagCategory = await TagCategoryRepository.CreateAsync(Fixture.Create<TagCategory>(), default);

        // Act
        var existingTagCategory = await TagCategoryRepository.GetByAsync(insertedTagCategory.TenantCode, insertedTagCategory.Code, default);

        // Assert
        existingTagCategory.Should().BeEquivalentTo(insertedTagCategory);
    }
    
    [TestMethod]
    public async Task GetByAsync_ByTenantCodeAndTagCategoryCode_Null()
    {
        // Arrange & Act
        var existingTagCategory = await TagCategoryRepository.GetByAsync(Fixture.Create<int>(), Fixture.Create<string>(), default);

        // Assert
        existingTagCategory.Should().BeNull();
    }
    
    #endregion
    
    #region ExistsAsync
    
    [TestMethod]
    public async Task ExistsAsync_ByTenantCodeAndTagCategoryCode_Exists()
    {
        // Arrange
        var insertedTagCategory = await TagCategoryRepository.CreateAsync(Fixture.Create<TagCategory>(), default);

        // Act
        var tagCategoryExists = await TagCategoryRepository.ExistsAsync(insertedTagCategory.TenantCode, insertedTagCategory.Code, default);

        // Assert
        tagCategoryExists.Should().BeTrue();
    }
    
    [TestMethod]
    public async Task ExistsAsync_ByTenantCodeAndTagCategoryCode_NotExists()
    {
        // Arrange
        var insertedTagCategory = await TagCategoryRepository.CreateAsync(Fixture.Create<TagCategory>(), default);

        // Act
        var tagCategoryExists = await TagCategoryRepository.ExistsAsync(insertedTagCategory.TenantCode, Fixture.Create<string>(), default);

        // Assert
        tagCategoryExists.Should().BeFalse();
    }
    
    #endregion
    
    
    #region RenameAsync
    
    [TestMethod]
    public async Task RenameAsync_ByTenantCodeAndTagCategoryCode_Renames()
    {
        // Arrange
        var insertedTagCategory = await TagCategoryRepository.CreateAsync(Fixture.Create<TagCategory>(), default);

        var newTagCategoryCode = Fixture.Create<string>();

        // Act
        await TagCategoryRepository.RenameAsync(insertedTagCategory.TenantCode, insertedTagCategory.Code, newTagCategoryCode, default);

        // Assert
        var tagCategoryExists = await TagCategoryRepository.GetByAsync(insertedTagCategory.TenantCode, newTagCategoryCode, default);
        tagCategoryExists.Should().NotBeNull();
        tagCategoryExists!.Code.Should().Be(newTagCategoryCode);
        
        var oldTagCategoryExists = await TagCategoryRepository.ExistsAsync(insertedTagCategory.TenantCode, insertedTagCategory.Code, default);
        oldTagCategoryExists.Should().BeFalse();
    }
    
    #endregion
}