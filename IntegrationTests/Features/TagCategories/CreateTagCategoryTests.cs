using Core.Features.TagCategories;

namespace IntegrationTests.Features.TagCategories;

[Trait(nameof(Constants.Features), Constants.Features.TagCategories)]
public class CreateTagCategoryTests : IntegrationTestBase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITagCategoryRepository _tagCategoryRepository;

    public CreateTagCategoryTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
        _tagCategoryRepository = GetService<ITagCategoryRepository>();
    }

    [Fact]
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
        var tagCategoryExists = await _tagCategoryRepository.ExistsByCodeAsync(tagCategory.TagCategoryCode, default);
        Assert.True(tagCategoryExists);
    }
}