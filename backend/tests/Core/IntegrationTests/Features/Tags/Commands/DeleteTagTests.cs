using Core.Features.Tags.Commands;

namespace IntegrationTests.Features.Tags.Commands;

[TestClass]
public class DeleteTagTests : BaseIntegrationTest
{
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();

    [TestMethod]
    public async Task Handle_DeleteTagInDatabase()
    {
        // Arrange
        var tenant = await MongoDbFixture.CreateTenantAsync();
        var tagToInsert = Fixture
            .Build<Tag>()
            .With(tc => tc.TenantCode, tenant.Code)
            .Create();

        await _tagRepository.CreateAsync(tagToInsert, default);

        var handler = new DeleteTag.Handler(_tagRepository);

        // Act
        var tagDeletedDto = await handler.Handle(
            new DeleteTag.Command(tenant.Code, tagToInsert.TagCategoryCode, tagToInsert.Code),
            default);

        // Assert
        tagDeletedDto.IsDeleted.Should().BeTrue();
        
        var deletedTag = await _tagRepository.ExistsAsync(
            tenant.Code,
            tagToInsert.Code,
            default);

        deletedTag.Should().BeFalse();
    }
}