﻿using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands;

[TestClass]
public class CreateTagTestBaseHandlerTests : TestBaseHandler<CreateTagHandler.Handler, CreateTagHandler.Command>
{
    [TestMethod]
    public async Task Handle_WhenTenantDoesNotExist_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TenantNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagCategoryRepositoryMock.DidNotReceive().ExistsAsync(Request.TenantCode, Request.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Tag>(), Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_WhenTagCategoryDoesNotExist_ThrowsTagCategoryNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Request.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Request.TenantCode, Request.TagCategoryCode, default)
            .Returns(false);

        // Act and Assert
        await AssertThrowsAsync<TagCategoryNotFoundException>(Request);
        await TenantRepositoryMock.Received().ExistsAsync(Request.TenantCode, default);
        await TagCategoryRepositoryMock.Received().ExistsAsync(Request.TenantCode, Request.TagCategoryCode, default);
        await TagRepositoryMock.DidNotReceiveWithAnyArgs().CreateAsync(Arg.Any<Tag>(), Arg.Any<CancellationToken>());
    }
}