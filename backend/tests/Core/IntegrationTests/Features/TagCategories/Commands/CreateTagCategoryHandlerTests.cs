using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryHandlerTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_CreatesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var tagCategory = Fixture.Create<TagCategory>();

        var handler = new CreateTagCategoryHandler.Handler(TenantRepository, TagCategoryRepository);

        // Act
        await handler.Handle(new CreateTagCategoryHandler.Command(tenant.Code, tagCategory.Code), default);

        // Assert
        var tagCategoryExists =
            await TagCategoryRepository.ExistsAsync(tenant.Code, tagCategory.Code, default);
        tagCategoryExists.Should().BeTrue();
    }
}