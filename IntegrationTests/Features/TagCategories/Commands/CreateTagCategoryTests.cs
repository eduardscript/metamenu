using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_CreatesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var tagCategory = Fixture.Create<TagCategory>();

        var handler = new CreateTagCategory.Handler(TenantRepository, TagCategoryRepository);

        // Act
        await handler.Handle(new CreateTagCategory.Command(tenant.TenantCode, tagCategory.TagCategoryCode), default);

        // Assert
        var tagCategoryExists =
            await TagCategoryRepository.ExistsByAsync(tenant.TenantCode, tagCategory.TagCategoryCode, default);
        tagCategoryExists.Should().BeTrue();
    }
}