using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands;

[TestClass]
public class CreateTagTests : TestBase<CreateTag.Handler>
{
    private static readonly Tag Tag = Fixture.Create<Tag>();

    private readonly CreateTag.Command _command = new(Tag.TenantCode, Tag.TagCategoryCode, Tag.TagCode);

    public CreateTagTests()
    {
        Handler = new CreateTag.Handler(TenantRepositoryMock, TagCategoryRepositoryMock, TagRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(Tag.TenantCode, default).Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(Tag.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().ExistsByAsync(Tag.TenantCode, Tag.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceive().CreateAsync(Tag, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryDoesNotExist_ThrowsTagCategoryNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(Tag.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsByAsync(Tag.TenantCode, Tag.TagCategoryCode, default)
            .Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act and Assert
        await action.Should().ThrowAsync<TagCategoryNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(Tag.TenantCode, default);
        await TagCategoryRepositoryMock.Received().ExistsByAsync(Tag.TenantCode, Tag.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceive().CreateAsync(Tag, default);
    }
}