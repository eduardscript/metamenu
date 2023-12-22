using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class CreateTagTests : IntegrationTestBase
{
    private readonly ITagCategoryRepository _tagCategoryRepository = GetService<ITagCategoryRepository>();
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();
    private readonly ITenantRepository _tenantRepository = GetService<ITenantRepository>();

    [TestMethod]
    public async Task Handle_CreatesTagInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

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