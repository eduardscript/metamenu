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
        TenantRepositoryMock.ExistsByCodeAsync(Product.TenantCode, default).Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(Product.TenantCode, default);
        await TagRepositoryMock.DidNotReceive().ExistsByCodeAsync(Product.TenantCode, Product.TagCodes, default);
        await ProductRepositoryMock.DidNotReceive().CreateAsync(Product, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagDoesNotExist_ThrowsTagNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByCodeAsync(Product.TenantCode, default).Returns(true);
        TagRepositoryMock.ExistsByCodeAsync(Product.TenantCode, Product.TagCodes, default).Returns(false);

        var action = new Func<Task>(async () => await Handler.Handle(_command, default));

        // Act and Assert
        await action.Should().ThrowAsync<TagNotFoundException>();
        await TenantRepositoryMock.Received().ExistsByCodeAsync(Product.TenantCode, default);
        await TagRepositoryMock.Received().ExistsByCodeAsync(Product.TenantCode, Product.TagCodes, default);
        await ProductRepositoryMock.DidNotReceive().CreateAsync(Product, default);
    }
}