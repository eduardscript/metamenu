using Core.Features.TagCategories.Commands;

namespace IntegrationTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTests : IntegrationTestBase
{
    private readonly ITagCategoryRepository _tagCategoryRepository;
    private readonly ITenantRepository _tenantRepository;

    public CreateTagCategoryTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
        _tagCategoryRepository = GetService<ITagCategoryRepository>();
    }

    [TestMethod]
    public async Task Handle_CreatesTagCategoryInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();
        await _tenantRepository.CreateAsync(tenant, default);

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