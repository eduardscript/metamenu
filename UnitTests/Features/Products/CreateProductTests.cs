using Core.Features.Products;

namespace UnitTests.Features.Products;

[Trait(nameof(Constants.Features), Constants.Features.Products)]
public class CreateProductTests : TestBase
{
    private readonly Product _product = Fixture.Create<Product>();

    [Fact]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_product.TenantCode, default).Returns(false);

        var tagRepository = Substitute.For<ITagRepository>();

        var productRepository = Substitute.For<IProductRepository>();

        var handler = new CreateProduct.Handler(tenantRepository, tagRepository, productRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TenantNotFoundException>(() => handler.Handle(new CreateProduct.Command(
                _product.TenantCode,
                _product.Name,
                _product.Description,
                _product.Price,
                _product.TagCodes),
            default));

        await productRepository.DidNotReceive().CreateAsync(_product, default);
    }

    [Fact]
    public async Task Handle_WhenTagDoesNotExist_ThrowsTagNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_product.TenantCode, default).Returns(true);

        var tagRepository = Substitute.For<ITagRepository>();
        tagRepository.ExistsByCodeAsync(_product.TagCodes, default).Returns(false);

        var productRepository = Substitute.For<IProductRepository>();

        var handler = new CreateProduct.Handler(tenantRepository, tagRepository, productRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TagNotFoundException>(() => handler.Handle(new CreateProduct.Command(
                _product.TenantCode,
                _product.Name,
                _product.Description,
                _product.Price,
                _product.TagCodes),
            default));
    }
}