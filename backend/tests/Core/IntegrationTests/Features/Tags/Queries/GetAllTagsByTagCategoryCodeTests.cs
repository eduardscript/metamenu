using Core.Features.Tags.Queries;
using Core.Features.Tags.Shared;

namespace IntegrationTests.Features.Tags.Queries;

[TestClass]
public class GetAllTagsByTagCategoryCodeTests : BaseIntegrationTest
{
    private readonly ITagRepository _tagRepository = GetService<ITagRepository>();

    [TestMethod]
    public async Task Handle_ReturnsAllTagsForTenant()
    {
        // Arrange
        var tenantCode = Fixture.Create<int>();
        var tagCategoryCode = Fixture.Create<string>();

        var tagsToInsert = Fixture.Build<Tag>()
            .With(tag => tag.TenantCode, tenantCode)
            .With(tag => tag.TagCategoryCode, tagCategoryCode)
            .CreateMany()
            .ToList();

        foreach (var tagToInsert in tagsToInsert)
        {
            await _tagRepository.CreateAsync(tagToInsert, default);
        }

        var handler = new GetAllTagsByTagCategoryCode.Handler(_tagRepository);

        // Act
        var tagsDto = await handler.Handle(new GetAllTagsByTagCategoryCode.Query(tenantCode, tagCategoryCode), default);

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