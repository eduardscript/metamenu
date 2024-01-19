using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class CreateTagTests : BaseIntegrationTest
{
    [TestMethod]
    public async Task Handle_CreatesTagInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();

        var tagCategory = Fixture.Build<TagCategory>()
            .With(tc => tc.TenantCode, tenant.Code)
            .Create();

        await TagCategoryRepository.CreateAsync(tagCategory, default);

        var tag = Fixture.Build<Tag>()
            .With(t => t.TenantCode, tenant.Code)
            .With(t => t.TagCategoryCode, tagCategory.Code)
            .Create();

        var handler = new CreateTag.Handler(TagRepository);

        // Act
        await handler.Handle(new CreateTag.Command(tag.TenantCode, tag.TagCategoryCode, tag.Code), default);

        // Assert
        var tagExists = await TagRepository.ExistsAsync(tag.TenantCode, tag.Code, default);
        tagExists.Should().BeTrue();
    }
}