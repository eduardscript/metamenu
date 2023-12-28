using Core.Exceptions.Products;
using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Products.Commands;

namespace UnitTests.Features.Products.Commands;

[TestClass]
public class UpdateProductTestBaseHandlerTests : TestBaseHandler<UpdateProductTestBaseHandler.Handler, UpdateProductTestBaseHandler.Command>
{
    private static readonly Product Product = Fixture.Build<Product>()
        .With(p => p.TenantCode, Request.TenantCode)
        .With(p => p.Name, Request.Name)
        .Create();

    public UpdateProductTestBaseHandlerTests()
    {
        Handler = new UpdateProductTestBaseHandler.Handler(TenantRepositoryMock, TagRepositoryMock, ProductRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(Request.TenantCode, default).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);
    }

    [TestMethod]
    public async Task Handle_ProductNotFound_ThrowsProductNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(Request.TenantCode, default).Returns(true);
        ProductRepositoryMock.GetByAsync(Request.TenantCode, Request.Name, default).Returns((Product)null!);

        // Act & Assert
        await AssertThrowsAsync<ProductNotFoundException>(Request);
        
        await ProductRepositoryMock.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<string>(),  Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
    
    [TestMethod]
    public async Task Handle_ProductNameAlreadyExists_ThrowsProductAlreadyExists()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(Request.TenantCode, default).Returns(true);
        ProductRepositoryMock.GetByAsync(Request.TenantCode, Request.Name, default).Returns(Product);
        ProductRepositoryMock.ExistsByNameAsync(Request.TenantCode, Request.UpdateProperties.Name!, default).Returns(true);

        // Act & Assert
        await AssertThrowsAsync<ProductAlreadyExistsException>(Request);
        
        await ProductRepositoryMock.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<string>(),  Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TagNotFound_ThrowsTagNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(Request.TenantCode, default).Returns(true);
        ProductRepositoryMock.GetByAsync(Request.TenantCode, Request.Name, default).Returns(Product);
        TagRepositoryMock.ExistsAsync(Request.TenantCode, Request.UpdateProperties.TagCodes!, default).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TagNotFoundException>(Request);
        
        await ProductRepositoryMock.DidNotReceiveWithAnyArgs().UpdateAsync(Arg.Any<string>(),  Arg.Any<Product>(), Arg.Any<CancellationToken>());
    }
}