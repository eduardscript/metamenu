using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands;

[TestClass]
public class CreateTagTestBaseHandlerTests : TestBaseHandler<CreateTagHandler.Handler, CreateTagHandler.Command>
{
    public CreateTagTestBaseHandlerTests()
    {
        Handler = new CreateTagHandler.Handler(TenantRepositoryMock, TagCategoryRepositoryMock, TagRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().ExistsByAsync(Request.TenantCode, Request.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Tag>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryDoesNotExist_ThrowsTagCategoryNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsByAsync(Request.TenantCode, Request.TagCategoryCode, default)
            .Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TagCategoryNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagCategoryRepositoryMock.Received().ExistsByAsync(Request.TenantCode, Request.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Tag>(), Arg.Any<CancellationToken>());
    }
}