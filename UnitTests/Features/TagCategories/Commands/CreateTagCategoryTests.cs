using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands;

[TestClass]
public class CreateTagCategoryTests : TestBase<CreateTagCategory.Handler>
{
    private static readonly TagCategory TagCategory = Fixture.Create<TagCategory>();

    private readonly CreateTagCategory.Command _command = new(TagCategory.TenantCode, TagCategory.TagCategoryCode);

    public CreateTagCategoryTests()
    {
        Handler = new CreateTagCategory.Handler(TenantRepositoryMock, TagCategoryRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(TagCategory.TenantCode, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TenantNotFoundException>(_command);
        await TenantRepositoryMock.Received().ExistsByAsync(TagCategory.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().CreateAsync(TagCategory, default);
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(TagCategory.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsByAsync(TagCategory.TenantCode, TagCategory.TagCategoryCode, default)
            .Returns(true);

        // Act and Assert
        await AssertThrowsAsync<TagCategoryAlreadyExistsException>(_command);
        await TenantRepositoryMock.Received().ExistsByAsync(TagCategory.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().CreateAsync(TagCategory, default);
    }
}