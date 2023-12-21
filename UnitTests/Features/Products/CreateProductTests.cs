using Core.Features.Products.Commands;

namespace UnitTests.Features.Products;

[TestClass]
public class CreateProductTests : TestBase
{
    private readonly Product _product = Fixture.Create<Product>();

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_product.TenantCode, default).Returns(false);

        var tagRepository = Substitute.For<ITagRepository>();

        var productRepository = Substitute.For<IProductRepository>();

        var handler = new CreateProduct.Handler(tenantRepository, tagRepository, productRepository);

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateProduct.Command(
                    _product.TenantCode,
                    _product.Name,
                    _product.Description,
                    _product.Price,
                    _product.TagCodes),
                default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantNotFoundException>();

        await tenantRepository.Received().ExistsByCodeAsync(_product.TenantCode, default);
        await tagRepository.DidNotReceive().ExistsByCodeAsync(_product.TenantCode, _product.TagCodes, default);
        await productRepository.DidNotReceive().CreateAsync(_product, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagDoesNotExist_ThrowsTagNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_product.TenantCode, default).Returns(true);

        var tagRepository = Substitute.For<ITagRepository>();
        tagRepository.ExistsByCodeAsync(_product.TenantCode, _product.TagCodes, default).Returns(false);

        var productRepository = Substitute.For<IProductRepository>();

        var handler = new CreateProduct.Handler(tenantRepository, tagRepository, productRepository);

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateProduct.Command(
                    _product.TenantCode,
                    _product.Name,
                    _product.Description,
                    _product.Price,
                    _product.TagCodes),
                default));

        // Act and Assert
        await action.Should().ThrowAsync<TagNotFoundException>();

        await tenantRepository.Received().ExistsByCodeAsync(_product.TenantCode, default);
        await tagRepository.Received().ExistsByCodeAsync(_product.TenantCode, _product.TagCodes, default);
        await productRepository.DidNotReceive().CreateAsync(_product, default);
    }
}