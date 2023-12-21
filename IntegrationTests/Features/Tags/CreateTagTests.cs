using Core.Features.Tags;

namespace IntegrationTests.Features.Tags;

[Trait(nameof(Constants.Features), Constants.Features.Tags)]
public class CreateTagTests : IntegrationTestBase
{
    private readonly ITenantRepository _tenantRepository;
    private readonly ITagCategoryRepository _tagCategoryRepository;
    private readonly ITagRepository _tagRepository;

    public CreateTagTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
        _tagCategoryRepository = GetService<ITagCategoryRepository>();
        _tagRepository = GetService<ITagRepository>();
    }

    [Fact]
    public async Task Handle_CreatesTagInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();
        await _tenantRepository.CreateAsync(tenant, default);

        var tagCategory = Fixture.Create<TagCategory>();
        await _tagCategoryRepository.CreateAsync(tagCategory, default);

        var tag = Fixture.Build<Tag>()
            .With(t => t.TenantCode, tenant.TenantCode)
            .With(t => t.TagCategoryCode, tagCategory.TagCategoryCode)
            .Create();

        var handler = new CreateTag.Handler(_tenantRepository, _tagCategoryRepository, _tagRepository);

        // Act
        await handler.Handle(new CreateTag.Command(tag.TenantCode, tag.Code, tag.TagCategoryCode), default);

        // Assert
        var tagExists = await _tagRepository.ExistsByCodeAsync(tag.Code, default);
        Assert.True(tagExists);
    }
}
