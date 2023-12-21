using Core.Features.TagCategories;

namespace UnitTests.Features.TagCategories;

[Trait(nameof(Constants.Features), Constants.Features.TagCategories)]
public class CreateTagCategoryTests : TestBase
{
    private readonly TagCategory _tagCategory = Fixture.Create<TagCategory>();

    [Fact]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tagCategory.TenantCode, default).Returns(false);
        
        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();

        var handler = new CreateTagCategory.Handler(tenantRepository, tagCategoryRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TenantNotFoundException>(() => handler.Handle(new CreateTagCategory.Command(_tagCategory.TenantCode, _tagCategory.TagCategoryCode), default));
        
        await tagCategoryRepository.DidNotReceive().CreateAsync(_tagCategory, default);
    }

    [Fact]
    public async Task Handle_WhenTagCategoryExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tagCategory.TenantCode, default).Returns(true);

        var tagCategoryRepository = Substitute.For<ITagCategoryRepository>();
        tagCategoryRepository.ExistsByCodeAsync(_tagCategory.TagCategoryCode, default).Returns(true);

        var handler = new CreateTagCategory.Handler(tenantRepository, tagCategoryRepository);

        // Act and Assert
        await Assert.ThrowsAsync<TagCategoryAlreadyExistsException>(() => handler.Handle(new CreateTagCategory.Command(_tagCategory.TenantCode, _tagCategory.TagCategoryCode), default));
    }
}