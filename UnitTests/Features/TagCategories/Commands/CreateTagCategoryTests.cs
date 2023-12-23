using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTests : TestBase<CreateTagCategory.Handler>
{
    private static readonly TagCategory TagCategory = Fixture.Create<TagCategory>();

    private readonly CreateTagCategory.Command _command = new(TagCategory.TenantCode, TagCategory.TagCategoryCode);

    public CreateTagCategoryTests()
    {
        Handler = new CreateTagCategory.Handler(TenantRepositoryMock, TagCategoryRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(TagCategory.TenantCode, default).Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(TagCategory.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().CreateAsync(TagCategory, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(TagCategory.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsByAsync(TagCategory.TenantCode, TagCategory.TagCategoryCode, default)
            .Returns(true);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act and Assert
        await action.Should().ThrowAsync<TagCategoryAlreadyExistsException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(TagCategory.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().CreateAsync(TagCategory, default);
    }
}