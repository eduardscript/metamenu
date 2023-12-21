using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories;

[TestClass]
public class CreateTagCategoryTests : TestBase
{
    private readonly TagCategory _tagCategory = Fixture.Create<TagCategory>();

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tagCategory.TenantCode, default).Returns(false);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();

        var handler = new CreateTagCategory.Handler(tenantRepository, tagCategoryRepository);

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateTagCategory.Command(_tagCategory.TenantCode, _tagCategory.TagCategoryCode),
                default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();

        await tenantRepository.Received().ExistsByCodeAsync(_tagCategory.TenantCode, default);
        await tagCategoryRepository.DidNotReceive().CreateAsync(_tagCategory, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tagCategory.TenantCode, default).Returns(true);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();
        tagCategoryRepository.ExistsByAsync(_tagCategory.TenantCode, _tagCategory.TagCategoryCode, default)
            .Returns(true);

        var handler = new CreateTagCategory.Handler(tenantRepository, tagCategoryRepository);

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateTagCategory.Command(_tagCategory.TenantCode, _tagCategory.TagCategoryCode),
                default));

        // Act and Assert
        await action.Should().ThrowAsync<TagCategoryAlreadyExistsException>();

        await tenantRepository.Received().ExistsByCodeAsync(_tagCategory.TenantCode, default);
        await tagCategoryRepository.DidNotReceive().CreateAsync(_tagCategory, default);
    }
}