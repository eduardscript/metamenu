using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTestBaseHandlerTests : TestBaseHandler<CreateTagCategoryHandler.Handler, CreateTagCategoryHandler.Command>
{
    public CreateTagCategoryTestBaseHandlerTests()
    {
        Handler = new CreateTagCategoryHandler.Handler(TenantRepositoryMock, TagCategoryRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<TagCategory>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsByAsync(Request.TenantCode, Request.Code, default)
            .Returns(true);

        // Act and Assert
        await AssertThrowsAsync<TagCategoryAlreadyExistsException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<TagCategory>(), Arg.Any<CancellationToken>());
    }
}