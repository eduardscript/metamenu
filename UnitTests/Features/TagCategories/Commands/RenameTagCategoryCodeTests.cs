using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands;

[TestClass]
public class RenameTagCodeTestBaseHandlerTests : TestBaseHandler<RenameTagCategoryCode.Handler, RenameTagCategoryCode.Command>
{
    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>()).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().ExistsAsync(Request.TenantCode,
            Request.NewTagCategoryCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceiveWithAnyArgs().RenameAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TagCategoryAlreadyExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>()).Returns(true);
        TagCategoryRepositoryMock
            .ExistsAsync(Request.TenantCode, Request.NewTagCategoryCode, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act & Assert
        await AssertThrowsAsync<TagCategoryAlreadyExistsException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceiveWithAnyArgs().RenameAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>(), Arg.Any<CancellationToken>());
    }
}