using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTests : IntegrationTestBase
{
    private readonly ITagCategoryRepository _tagCategoryRepository = GetService<ITagCategoryRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_CreatesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var tagCategory = Fixture.Create<TagCategory>();

        var handler = new CreateTagCategory.Handler(_tenantRepository, _tagCategoryRepository);

        // Act
        await handler.Handle(new CreateTagCategory.Command(tenant.TenantCode, tagCategory.TagCategoryCode), default);

        // Assert
        var tagCategoryExists =
            await _tagCategoryRepository.ExistsByAsync(tenant.TenantCode, tagCategory.TagCategoryCode, default);
        tagCategoryExists.Should().BeTrue();
    }
}