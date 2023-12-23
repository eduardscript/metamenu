using Core.Exceptions.TagCategories;
using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands;

[TestClass]
public class RenameTagCodeTests : TestBase<RenameTagCode.Handler>
{
    private readonly RenameTagCode.Command _command = Fixture.Create<RenameTagCode.Command>();

    public RenameTagCodeTests()
    {
        Handler = new RenameTagCode.Handler(TenantRepositoryMock, TagRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>()).Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act & Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().ExistsByAsync(_command.TenantCode,
            _command.NewTagCode, Arg.Any<CancellationToken>());
        await TagRepositoryMock.DidNotReceive().RenameAsync(_command.TenantCode,
            _command.OldTagCode, _command.NewTagCode, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TagAlreadyExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>()).Returns(true);
        TagRepositoryMock
            .ExistsAsync(_command.TenantCode, _command.NewTagCode, Arg.Any<CancellationToken>())
            .Returns(true);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act & Assert
        await action.Should().ThrowAsync<TagAlreadyExistsException>();
        await TenantRepositoryMock.Received().ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>());
        await TagRepositoryMock.Received().ExistsAsync(_command.TenantCode,
            _command.NewTagCode, Arg.Any<CancellationToken>());
        await TagRepositoryMock.DidNotReceive().RenameAsync(_command.TenantCode,
            _command.OldTagCode, _command.NewTagCode, Arg.Any<CancellationToken>());
    }
}