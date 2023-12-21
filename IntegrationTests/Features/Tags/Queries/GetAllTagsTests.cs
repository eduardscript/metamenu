using Core.Features.Tags.Queries;

namespace IntegrationTests.Features.Tags.Queries;

[TestClass]
public class GetAllTagsTests : IntegrationTestBase
{
    private readonly ITagRepository _tagRepository;

    public GetAllTagsTests()
    {
        _tagRepository = GetService<ITagRepository>();
    }

    [TestMethod]
    public async Task Handle_ReturnsAllTagsForTenant()
    {
        // Arrange
        var tenantCode = Fixture.Create<int>();
        var expectedTags = Fixture.Build<Tag>()
            .With(tag => tag.TenantCode, tenantCode)
            .CreateMany()
            .ToList();

        foreach (var tag in expectedTags) await _tagRepository.CreateAsync(tag, default);

        var handler = new GetAllTags.Handler(_tagRepository);

        // Act
        var result = await handler.Handle(new GetAllTags.Query(tenantCode), default);

        // Assert
        var resultList = result.ToList();
        resultList.Should().HaveCount(expectedTags.Count);
        foreach (var expectedTag in expectedTags)
            resultList.Should().ContainEquivalentOf(new GetAllTags.TagDto(expectedTag.TenantCode, expectedTag.TagCode,
                expectedTag.TagCategoryCode));
    }
}