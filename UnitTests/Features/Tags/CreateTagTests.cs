using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags;

[TestClass]
public class CreateTagTests : TestBase
{
    private readonly Tag _tag = Fixture.Create<Tag>();

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tag.TenantCode, default).Returns(false);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();

        var tagRepository = Substitute.For<ITagRepository>();

        var handler = new CreateTag.Handler(tenantRepository, tagCategoryRepository, tagRepository);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TenantNotFoundException>(() =>
            handler.Handle(new CreateTag.Command(_tag.TenantCode, _tag.TagCategoryCode, _tag.TagCode), default));

        await tagRepository.DidNotReceive().CreateAsync(_tag, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryDoesNotExist_ThrowsTagCategoryNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tag.TenantCode, default).Returns(true);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();
        var tagCategory = Fixture.Create<TagCategory>();
        tagCategoryRepository.ExistsByAsync(tagCategory.TenantCode, tagCategory.TagCategoryCode, default)
            .Returns(false);

        var tagRepository = Substitute.For<ITagRepository>();

        var handler = new CreateTag.Handler(tenantRepository, tagCategoryRepository, tagRepository);

        // Act and Assert
        await Assert.ThrowsExceptionAsync<TagCategoryNotFoundException>(() =>
            handler.Handle(new CreateTag.Command(_tag.TenantCode, _tag.TagCategoryCode, _tag.TagCategoryCode),
                default));

        await tagRepository.DidNotReceive().CreateAsync(_tag, default);
    }
}