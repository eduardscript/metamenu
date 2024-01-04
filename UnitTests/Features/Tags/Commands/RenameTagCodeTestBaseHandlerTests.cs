using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands;

[TestClass]
public class RenameTagCodeTestBaseHandlerTests : TestBaseHandler<RenameTagCodeHandler.Handler, RenameTagCodeHandler.Command>
{
    public RenameTagCodeTestBaseHandlerTests()
    {
        Handler = new RenameTagCodeHandler.Handler(TenantRepositoryMock, TagRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>()).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);

        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().ExistsByAsync(Request.TenantCode,
            Request.NewTagCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceiveWithAnyArgs().RenameAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TagAlreadyExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>()).Returns(true);
        TagRepositoryMock
            .ExistsAsync(Request.TenantCode, Request.NewTagCode, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act & Assert
        await AssertThrowsAsync<TagAlreadyExistsException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>());
        await TagRepositoryMock.Received().ExistsAsync(Request.TenantCode,
            Request.NewTagCode, Arg.Any<CancellationToken>());
        
        await TagCategoryRepositoryMock.DidNotReceiveWithAnyArgs().RenameAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}