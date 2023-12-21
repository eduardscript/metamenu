using Core.Features.Tags;

namespace UnitTests.Features.Tags;

[Trait(nameof(Constants.Features), Constants.Features.Tags)]
public class CreateTagTests : TestBase
{
    private readonly Tag _tag = Fixture.Create<Tag>();

    [Fact]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tag.TenantCode, default).Returns(false);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();

        var tagRepository = Substitute.For<ITagRepository>();

        var handler = new CreateTag.Handler(tenantRepository, tagCategoryRepository, tagRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TenantNotFoundException>(() =>
            handler.Handle(new CreateTag.Command(_tag.TenantCode, _tag.TagCategoryCode, _tag.Code), default));

        await tagRepository.DidNotReceive().CreateAsync(_tag, default);
    }

    [Fact]
    public async Task Handle_WhenTagCategoryDoesNotExist_ThrowsTagCategoryNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tag.TenantCode, default).Returns(true);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();
        tagCategoryRepository.ExistsByCodeAsync(_tag.TagCategoryCode, default).Returns(false);

        var tagRepository = Substitute.For<ITagRepository>();

        var handler = new CreateTag.Handler(tenantRepository, tagCategoryRepository, tagRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TagCategoryNotFoundException>(() =>
            handler.Handle(new CreateTag.Command(_tag.TenantCode, _tag.TagCategoryCode, _tag.TagCategoryCode),
                default));

        await tagRepository.DidNotReceive().CreateAsync(_tag, default);
    }
}