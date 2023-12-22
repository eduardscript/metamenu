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

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateTag.Command(_tag.TenantCode, _tag.TagCategoryCode, _tag.TagCode),
                default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();

        await tenantRepository.Received().ExistsByCodeAsync(_tag.TenantCode, default);
        await tagCategoryRepository.DidNotReceive().ExistsByAsync(_tag.TenantCode, _tag.TagCategoryCode, default);
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

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateTag.Command(_tag.TenantCode, _tag.TagCategoryCode, _tag.TagCode),
                default));

        // Act and Assert
        await action.Should().ThrowAsync<TagCategoryNotFoundException>();

        await tenantRepository.Received().ExistsByCodeAsync(_tag.TenantCode, default);
        await tagCategoryRepository.Received().ExistsByAsync(_tag.TenantCode, _tag.TagCategoryCode, default);
        await tagRepository.DidNotReceive().CreateAsync(_tag, default);
    }
}