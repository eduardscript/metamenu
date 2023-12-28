using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class CreateTagHandlerTests : IntegrationTestBase
{
    [TestMethod]
    public async Task Handle_CreatesTagInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var tagCategory = Fixture.Build<TagCategory>()
            .With(tc => tc.TenantCode, tenant.TenantCode)
            .Create();

        await TagCategoryRepository.CreateAsync(tagCategory, default);

        var tag = Fixture.Build<Tag>()
            .With(t => t.TenantCode, tenant.TenantCode)
            .With(t => t.TagCategoryCode, tagCategory.TagCategoryCode)
            .Create();

        var handler = new CreateTagHandler.Handler(TenantRepository, TagCategoryRepository, TagRepository);

        // Act
        await handler.Handle(new CreateTagHandler.Command(tag.TenantCode, tag.TagCategoryCode, tag.TagCode), default);

        // Assert
        var tagExists = await TagRepository.ExistsAsync(tag.TenantCode, tag.TagCode, default);
        tagExists.Should().BeTrue();
    }
}