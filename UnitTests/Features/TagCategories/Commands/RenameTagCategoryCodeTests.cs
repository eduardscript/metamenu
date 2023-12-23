using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands;

[TestClass]
public class RenameTagCategoryCodeTests : TestBase<RenameTagCategoryCode.Handler>
{
    private readonly RenameTagCategoryCode.Command _command = Fixture.Create<RenameTagCategoryCode.Command>();

    public RenameTagCategoryCodeTests()
    {
        Handler = new RenameTagCategoryCode.Handler(TenantRepositoryMock, TagCategoryRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(_command.TenantCode, Arg.Any<CancellationToken>()).Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act & Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(_command.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().ExistsByAsync(_command.TenantCode,
            _command.NewTagCategoryCodeName, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TagCategoryAlreadyExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(_command.TenantCode, Arg.Any<CancellationToken>()).Returns(true);
        TagCategoryRepositoryMock
            .ExistsByAsync(_command.TenantCode, _command.NewTagCategoryCodeName, Arg.Any<CancellationToken>())
            .Returns(true);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act & Assert
        await action.Should().ThrowAsync<TagCategoryAlreadyExistsException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(_command.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.Received().ExistsByAsync(_command.TenantCode, _command.NewTagCategoryCodeName,
            Arg.Any<CancellationToken>());
    }
}