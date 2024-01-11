using Core.Exceptions.Products;
using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.TagCategories.Queries;
using Core.Features.Users.Queries;
using NSubstitute.ReturnsExtensions;

namespace UnitTests.Features.Tags.Queries.GetTagCategoryAssociatedEntitiesTests;

[TestClass]
public class Handler : TestBaseHandler<GetTagCategoryAssociatedEntities.Handler, GetTagCategoryAssociatedEntities.Query>
{
    public Handler()
    {
        Handler = new GetTagCategoryAssociatedEntities.Handler(
            TenantRepositoryMock,
            TagCategoryRepositoryMock,
            TagRepositoryMock,
            ProductRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryDoesNotExist_ThrowsTagCategoryNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.GetByAsync(Request.TenantCode, Request.TagCategoryCode, default)
            .ReturnsNull();

        // Act and Assert
        await AssertThrowsAsync<TagCategoryNotFoundException>(Request);
        await TagCategoryRepositoryMock.Received().GetByAsync(Request.TenantCode, Request.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceiveWithAnyArgs()
            .GetAll(Arg.Any<ITagRepository.TagFilter>(), Arg.Any<CancellationToken>());
        await ProductRepositoryMock.DidNotReceiveWithAnyArgs()
            .GetAllAsync(Arg.Any<ProductFilter>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryHasNoAssociatedEntities_ThrowsTagCategoryHasNoAssociatedEntitiesException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.GetByAsync(Request.TenantCode, Request.TagCategoryCode, default)
            .Returns(Fixture.Create<TagCategory>());
        TagRepositoryMock.GetAll(Arg.Any<ITagRepository.TagFilter>(), Arg.Any<CancellationToken>())
            .Returns(Fixture.CreateMany<Tag>());
        ProductRepositoryMock.GetAllAsync(Arg.Any<ProductFilter>(), Arg.Any<CancellationToken>())
            .Returns(Enumerable.Empty<Product>());
        
        // Act and Assert
        await AssertThrowsAsync<TagCategoryHasNoAssociatedEntitiesException>(Request);
        
        await TagRepositoryMock.Received().GetAll(Arg.Any<ITagRepository.TagFilter>(), Arg.Any<CancellationToken>());
        await ProductRepositoryMock.Received().GetAllAsync(Arg.Any<ProductFilter>(), Arg.Any<CancellationToken>());
    }
}