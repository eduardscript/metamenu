using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_CreatesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var tagCategory = Fixture.Create<TagCategory>();

        var handler = new CreateTagCategory.Handler(TagCategoryRepository);

        // Act
        await handler.Handle(new CreateTagCategory.Command(tenant.Code, tagCategory.Code), default);

        // Assert
        var tagCategoryExists =
            await TagCategoryRepository.ExistsAsync(tenant.Code, tagCategory.Code, default);
        tagCategoryExists.Should().BeTrue();
    }
}