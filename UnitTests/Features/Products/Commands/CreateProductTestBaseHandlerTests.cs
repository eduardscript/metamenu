using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Products.Commands;

namespace UnitTests.Features.Products.Commands;

[TestClass]
public class CreateProductTestBaseHandlerTests : TestBaseHandler<CreateProductHandler.Handler, CreateProductHandler.Command>
{
    public CreateProductTestBaseHandlerTests()
    {
        Handler = new CreateProductHandler.Handler(TenantRepositoryMock, TagRepositoryMock, ProductRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagRepositoryMock.DidNotReceive().ExistsAsync(Request.TenantCode, Request.TagCodes, default);
        await ProductRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_WhenTagDoesNotExist_ThrowsTagNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(true);
        TagRepositoryMock.ExistsAsync(Request.TenantCode, Request.TagCodes, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TagNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagRepositoryMock.Received().ExistsAsync(Request.TenantCode, Request.TagCodes, default);
        await ProductRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}