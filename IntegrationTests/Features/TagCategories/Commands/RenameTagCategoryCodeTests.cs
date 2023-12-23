using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class RenameTagCategoryCodeTests : IntegrationTestBase
{
    private readonly ITagCategoryRepository _tagCategoryRepository = GetService<ITagCategoryRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

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

        await _tagCategoryRepository.CreateAsync(oldTagCategory, default);

        var handler = new RenameTagCategoryCode.Handler(_tenantRepository, _tagCategoryRepository);

        // Act
        await handler.Handle(
            new RenameTagCategoryCode.Command(tenant.TenantCode, oldTagCategory.TagCategoryCode, newTagCategoryCode),
            default);

        // Assert
        var oldTagCategoryExists =
            await _tagCategoryRepository.ExistsByAsync(tenant.TenantCode, oldTagCategory.TagCategoryCode, default);
        oldTagCategoryExists.Should().BeFalse();

        var newTagCategoryExists =
            await _tagCategoryRepository.ExistsByAsync(tenant.TenantCode, newTagCategoryCode, default);
        newTagCategoryExists.Should().BeTrue();

        newTagCategoryCode.Should().NotBeEquivalentTo(oldTagCategory.TagCategoryCode);
    }
}