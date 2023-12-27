using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;
using Core.Features.Products.Commands;

namespace UnitTests.Features.Products.Commands;

[TestClass]
public class CreateProductTests : TestBase<CreateProduct.Handler>
{
    private static readonly Product Product = Fixture.Create<Product>();

    private readonly CreateProduct.Command _command = new(
        Product.TenantCode,
        Product.Name,
        Product.Description,
        Product.Price,
        Product.TagCodes);

    public CreateProductTests()
    {
        Handler = new CreateProduct.Handler(TenantRepositoryMock, TagRepositoryMock, ProductRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(Product.TenantCode, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TenantNotFoundException>(_command);
        await TenantRepositoryMock.Received().ExistsByAsync(Product.TenantCode, default);
        await TagRepositoryMock.DidNotReceive().ExistsAsync(Product.TenantCode, Product.TagCodes, default);
        await ProductRepositoryMock.DidNotReceive().CreateAsync(Product, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagDoesNotExist_ThrowsTagNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(Product.TenantCode, default).Returns(true);
        TagRepositoryMock.ExistsAsync(Product.TenantCode, Product.TagCodes, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TagNotFoundException>(_command);
        await TenantRepositoryMock.Received().ExistsByAsync(Product.TenantCode, default);
        await TagRepositoryMock.Received().ExistsAsync(Product.TenantCode, Product.TagCodes, default);
        await ProductRepositoryMock.DidNotReceive().CreateAsync(Product, default);
    }
}