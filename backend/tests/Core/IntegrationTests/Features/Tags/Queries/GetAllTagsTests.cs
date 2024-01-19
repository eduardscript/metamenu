using Core.Features.Tags.Queries;
using Core.Features.Tags.Shared;

namespace IntegrationTests.Features.Tags.Queries;

[TestClass]
public class GetAllTagsTests : BaseIntegrationTest
{
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();

    [TestMethod]
    public async Task Handle_ReturnsAllTagsForTenant()
    {
        // Arrange
        var tenantCode = Fixture.Create<int>();

        var tagsToInsert = Fixture.Build<Tag>()
            .With(tag => tag.TenantCode, tenantCode)
            .CreateMany()
            .ToList();

        foreach (var tagToInsert in tagsToInsert)
        {
            await _tagRepository.CreateAsync(tagToInsert, default);
        }

        var handler = new GetAllTags.Handler(_tagRepository);

        // Act
        var tagsDto = await handler.Handle(new GetAllTags.Query(tenantCode), default);

        // Assert
        var tagsDtoList = tagsDto.ToList();

        tagsDtoList.Should().HaveCount(tagsToInsert.Count);

        foreach (var expectedTag in tagsToInsert)
        {
            tagsDtoList.Should().ContainEquivalentOf(
                new TagDto(
                    expectedTag.TenantCode,
                    expectedTag.TagCategoryCode,
                    expectedTag.Code));
        }
    }
}