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
            .With(tc => tc.TenantCode, tenant.Code)
            .Create();

        var newTagCategoryCode = Fixture.Create<string>();

        await TagCategoryRepository.CreateAsync(oldTagCategory, default);

        var handler = new RenameTagCategoryCode.Handler(TagCategoryRepository);

        // Act
        await handler.Handle(
            new RenameTagCategoryCode.Command(tenant.Code, oldTagCategory.Code, newTagCategoryCode),
            default);

        // Assert
        var oldTagCategoryExists =
            await TagCategoryRepository.ExistsAsync(tenant.Code, oldTagCategory.Code, default);
        oldTagCategoryExists.Should().BeFalse();

        var newTagCategoryExists =
            await TagCategoryRepository.ExistsAsync(tenant.Code, newTagCategoryCode, default);
        newTagCategoryExists.Should().BeTrue();

        newTagCategoryCode.Should().NotBeEquivalentTo(oldTagCategory.Code);
    }
}