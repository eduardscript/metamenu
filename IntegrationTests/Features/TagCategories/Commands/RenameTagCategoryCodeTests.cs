using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class RenameTagCategoryCodeTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_RenamesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();
        var oldTagCategory = Fixture
            .Build<TagCategory>()
            .With(tc => tc.TenantCode, tenant.TenantCode)
            .Create();

        var newTagCategoryCode = Fixture.Create<string>();

        await TagCategoryRepository.CreateAsync(oldTagCategory, default);

        var handler = new RenameTagCategoryCode.Handler(TenantRepository, TagCategoryRepository);

        // Act
        await handler.Handle(
            new RenameTagCategoryCode.Command(tenant.TenantCode, oldTagCategory.TagCategoryCode, newTagCategoryCode),
            default);

        // Assert
        var oldTagCategoryExists =
            await TagCategoryRepository.ExistsByAsync(tenant.TenantCode, oldTagCategory.TagCategoryCode, default);
        oldTagCategoryExists.Should().BeFalse();

        var newTagCategoryExists =
            await TagCategoryRepository.ExistsByAsync(tenant.TenantCode, newTagCategoryCode, default);
        newTagCategoryExists.Should().BeTrue();

        newTagCategoryCode.Should().NotBeEquivalentTo(oldTagCategory.TagCategoryCode);
    }
}