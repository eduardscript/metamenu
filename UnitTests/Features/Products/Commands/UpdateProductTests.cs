using Core.Exceptions.Products;
using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Products.Commands;

namespace UnitTests.Features.Products.Commands;

[TestClass]
public class UpdateProductTests : TestBase<UpdateProduct.Handler>
{
    private static readonly Product Product = Fixture.Create<Product>();

    private readonly UpdateProduct.Command _command = new(
        Product.TenantCode,
        Product.Name,
        Fixture.Create<UpdateProduct.UpdateProperties>());

    public UpdateProductTests()
    {
        Handler = new UpdateProduct.Handler(TenantRepositoryMock, TagRepositoryMock, ProductRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, default).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TenantNotFoundException>(_command);
    }

    [TestMethod]
    public async Task Handle_ProductNotFound_ThrowsProductNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, default).Returns(true);
        ProductRepositoryMock.GetByAsync(_command.TenantCode, _command.Name, default).Returns((Product)null!);

        // Act & Assert
        await AssertThrowsAsync<ProductNotFoundException>(_command);
    }
    
    [TestMethod]
    public async Task Handle_ProductNameAlreadyExists_ThrowsProductAlreadyExists()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, default).Returns(true);
        ProductRepositoryMock.GetByAsync(_command.TenantCode, _command.Name, default).Returns(Product);
        ProductRepositoryMock.ExistsByNameAsync(_command.TenantCode, _command.UpdateProperties.Name!, default).Returns(true);

        // Act & Assert
        await AssertThrowsAsync<ProductAlreadyExistsException>(_command);
    }

    [TestMethod]
    public async Task Handle_TagNotFound_ThrowsTagNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, default).Returns(true);
        ProductRepositoryMock.GetByAsync(_command.TenantCode, _command.Name, default).Returns(Product);
        TagRepositoryMock.ExistsAsync(_command.TenantCode, _command.UpdateProperties.TagCodes!, default).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TagNotFoundException>(_command);
    }
}