using Core.Features.Products.Commands;

namespace IntegrationTests.Features.Products.Commands;

[TestClass]
public class UpdateProductTests : IntegrationTestBase
{
    private static Tenant _tenant = default!;
    private static List<Tag> _tags = default!;
    private static Product _product = default!;
    private UpdateProduct.Handler _handler = default!;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _tenant = MongoDbFixture.CreateTenantAsync().GetAwaiter().GetResult();
        _handler = new UpdateProduct.Handler(TenantRepository, TagRepository, ProductRepository);
        
        _tags = Fixture.Build<Tag>()
                       .With(t => t.TenantCode, _tenant.TenantCode)
                       .CreateMany()
                       .ToList();

        foreach (var tag in _tags)
        {
            await TagRepository.CreateAsync(tag, default);
        }

        _product = Fixture.Build<Product>()
                                  .With(p => p.TenantCode, _tenant.TenantCode)
                                  .With(p => p.TagCodes, _tags.Select(t => t.TagCode))
                                  .Create();
        
        await ProductRepository.CreateAsync(_product, default);
    }

    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_FullUpdate()
    {
        // Arrange
        var updateProperties = Fixture.Build<UpdateProduct.UpdateProperties>()
            .With(p => p.TagCodes, _tags.Take(2).Select(t => t.TagCode))
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
        var updateProperties = new UpdateProduct.UpdateProperties
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
        var updateProperties = new UpdateProduct.UpdateProperties
        {
            Description = Fixture.Create<string>()
        };
        
        var expectedUpdatedProduct = new Product(
            _product.TenantCode,
            _product.Name!,
            updateProperties.Description,
            _product.Price,
            _product.TagCodes);

        // Act & Assert
        await ActAndAssert(_product, expectedUpdatedProduct, updateProperties);
    }
    
    [TestMethod]
    public async Task Handle_UpdatesProductInDatabase_PriceUpdate()
    {
        // Arrange
        var updateProperties = new UpdateProduct.UpdateProperties
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
        var updateProperties = new UpdateProduct.UpdateProperties
        {
            TagCodes = _tags.Take(2).Select(t => t.TagCode)
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
        UpdateProduct.UpdateProperties updateProperties)
    {
        await _handler.Handle(
            new UpdateProduct.Command(_tenant.TenantCode, _product.Name, updateProperties), 
            default);

        var updatedProductResponse = await ProductRepository.GetByAsync(updatedProduct.TenantCode, updatedProduct.Name!, default);
        updatedProduct.Should().BeEquivalentTo(updatedProductResponse);
        
        if (oldProduct.Name != updatedProduct.Name)
        {
            var oldProductResponse = await ProductRepository.ExistsByNameAsync(oldProduct.TenantCode, oldProduct.Name, default);
            oldProductResponse.Should().BeFalse();
        }
    }
}