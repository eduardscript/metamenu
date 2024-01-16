﻿using Core.Exceptions.TagCategories;
using Core.Features.TagCategories.Queries;

namespace UnitTests.Features.Tags.Queries.GetTagCategoryAssociatedEntitiesTests;

[TestClass]
public class Handler : TestBaseHandler<GetTagCategoryAssociatedEntities.Handler, GetTagCategoryAssociatedEntities.Query>
{
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