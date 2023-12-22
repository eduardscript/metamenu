using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class CreateTagTests : IntegrationTestBase
{
    private readonly ITagCategoryRepository _tagCategoryRepository;
    private readonly ITagRepository _tagRepository;
    private readonly ITenantRepository _tenantRepository;

    public CreateTagTests()
    {
        _tenantRepository = GetService<ITenantRepository>();
        _tagCategoryRepository = GetService<ITagCategoryRepository>();
        _tagRepository = GetService<ITagRepository>();
    }

    [TestMethod]
    public async Task Handle_CreatesTagInDatabase()
    {
        // Arrange
        var tenant = Fixture.Create<Tenant>();
        await _tenantRepository.CreateAsync(tenant, default);

        var tagCategory = Fixture.Build<TagCategory>()
            .With(tc => tc.TenantCode, tenant.TenantCode)
            .Create();

        await _tagCategoryRepository.CreateAsync(tagCategory, default);

        var tag = Fixture.Build<Tag>()
            .With(t => t.TenantCode, tenant.TenantCode)
            .With(t => t.TagCategoryCode, tagCategory.TagCategoryCode)
            .Create();

        var handler = new CreateTag.Handler(_tenantRepository, _tagCategoryRepository, _tagRepository);

        // Act
        await handler.Handle(new CreateTag.Command(tag.TenantCode, tag.TagCategoryCode, tag.TagCode), default);

        // Assert
        var tagExists = await _tagRepository.ExistsByCodeAsync(tag.TenantCode, tag.TagCode, default);
        tagExists.Should().BeTrue();
    }
}