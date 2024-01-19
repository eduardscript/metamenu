using Core.Features.Products.Commands;

namespace IntegrationTests.Features.Products.Commands;

[TestClass]
public class UpdateProductTestBaseHandlerTests : BaseIntegrationTest
{
    private static Tenant _tenant = default!;
    private static List<Tag> _tags = default!;
    private static Product _product = default!;
    private UpdateProductTestBaseHandler.Handler _handler = default!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _tenant = MongoDbFixture.CreateTenantAsync().GetAwaiter().GetResult();
        _handler = new UpdateProductTestBaseHandler.Handler(TenantRepository, TagRepository, ProductRepository);

        _tags = Fixture.Build<Tag>()
            .With(t => t.TenantCode, _tenant.Code)
            .CreateMany()
            .ToList();

        foreach (var tag in _tags)
        {
            await TagRepository.CreateAsync(tag, default);
        }

        _product = Fixture.Build<Product>()
            .With(p => p.TenantCode, _tenant.Code)
            .With(p => p.TagCodes, _tags.Select(t => t.Code))
            .Create();

        await ProductRepository.CreateAsync(_product, default);
    }

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_FullUpdate()
    {
        // Arrange
        var updateProperties = Fixture.Build<UpdateProductTestBaseHandler.UpdateProperties>()
            .With(p => p.TagCodes, _tags.Take(2).Select(t => t.Code))
            .Create();

        var expectedUpdatedProduct = new Product(
            _product.TenantCode,
            updateProperties.Name!,
            updateProperties.Description,
            updateProperties.Price!.Value,
            updateProperties.TagCodes!);

        // Act & Assert
        await ActAndAssert(_product, expectedUpdatedProduct, updateProperties);
    }

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_NameUpdate()
    {
        // Arrange
        var updateProperties = new UpdateProductTestBaseHandler.UpdateProperties
        {
            Name = Fixture.Create<string>()
        };

        var expectedUpdatedProduct = new Product(
            _product.TenantCode,
            updateProperties.Name!,
            _product.Description,
            _product.Price,
            _product.TagCodes);

        // Act & Assert
        await ActAndAssert(_product, expectedUpdatedProduct, updateProperties);
    }

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_DescriptionUpdate()
    {
        // Arrange
        var updateProperties = new UpdateProductTestBaseHandler.UpdateProperties
        {
            Description = Fixture.Create<string>()
        };

        var expectedUpdatedProduct = new Product(
            _product.TenantCode,
            _product.Name!,
            updateProperties.Description,
            _product.Price,
            _product.TagCodes)
        {
            Code = 1
        };

        // Act & Assert
        await ActAndAssert(_product, expectedUpdatedProduct, updateProperties);
    }

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_PriceUpdate()
    {
        // Arrange
        var updateProperties = new UpdateProductTestBaseHandler.UpdateProperties
        {
            Price = Fixture.Create<decimal>()
        };

        var expectedUpdatedProduct = new Product(
            _product.TenantCode,
            _product.Name,
            _product.Description,
            updateProperties.Price!.Value,
            _product.TagCodes);

        // Act & Assert
        await ActAndAssert(_product, expectedUpdatedProduct, updateProperties);
    }

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_TagCodesUpdate()
    {
        // Arrange
        var updateProperties = new UpdateProductTestBaseHandler.UpdateProperties
        {
            TagCodes = _tags.Take(2).Select(t => t.Code)
        };

        var expectedUpdatedProduct = new Product(
            _product.TenantCode,
            _product.Name,
            _product.Description,
            _product.Price,
            updateProperties!.TagCodes);

        // Act & Assert
        await ActAndAssert(_product, expectedUpdatedProduct, updateProperties);
    }


    private async Task ActAndAssert(
        Product oldProduct,
        Product updatedProduct,
        UpdateProductTestBaseHandler.UpdateProperties updateProperties)
    {
        await _handler.Handle(
            new UpdateProductTestBaseHandler.Command(_tenant.Code, _product.Name, updateProperties),
            default);

        var updatedProductResponse =
            await ProductRepository.GetByAsync(updatedProduct.TenantCode, updatedProduct.Name!, default);

        updatedProduct.Should().BeEquivalentTo(
            updatedProductResponse,
            options => options.Excluding(p => p!.Code));

        if (oldProduct.Name != updatedProduct.Name)
        {
            var oldProductResponse =
                await ProductRepository.ExistsByNameAsync(oldProduct.TenantCode, oldProduct.Name, default);

            oldProductResponse.Should().BeFalse();
        }
    }
}