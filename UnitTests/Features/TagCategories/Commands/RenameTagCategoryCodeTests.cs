﻿using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands;

[TestClass]
public class RenameTagCodeTests : TestBase<RenameTagCategoryCode.Handler>
{
    private readonly RenameTagCategoryCode.Command _command = Fixture.Create<RenameTagCategoryCode.Command>();

    public RenameTagCodeTests()
    {
        Handler = new RenameTagCategoryCode.Handler(TenantRepositoryMock, TagCategoryRepositoryMock);
    }

    [TestMethod]
    public async Task Handle_TenantNotFound_ThrowsTenantNotFoundException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>()).Returns(false);

        // Act & Assert
        await AssertThrowsAsync<TenantNotFoundException>(_command);
        await TenantRepositoryMock.Received().ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().ExistsByAsync(_command.TenantCode,
            _command.NewTagCategoryCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().RenameAsync(_command.TenantCode,
            _command.OldTagCategoryCode, _command.NewTagCategoryCode, Arg.Any<CancellationToken>());
    }

    [TestMethod]
    public async Task Handle_TagCategoryAlreadyExists_ThrowsTagCategoryAlreadyExistsException()
    {
        // Arrange
        TenantRepositoryMock.ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>()).Returns(true);
        TagCategoryRepositoryMock
            .ExistsByAsync(_command.TenantCode, _command.NewTagCategoryCode, Arg.Any<CancellationToken>())
            .Returns(true);

        // Act & Assert
        await AssertThrowsAsync<TagCategoryAlreadyExistsException>(_command);
        await TenantRepositoryMock.Received().ExistsByAsync(_command.TenantCode, Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.Received().ExistsByAsync(_command.TenantCode, _command.NewTagCategoryCode,
            Arg.Any<CancellationToken>());
        await TagCategoryRepositoryMock.DidNotReceive().RenameAsync(_command.TenantCode,
            _command.OldTagCategoryCode, _command.NewTagCategoryCode, Arg.Any<CancellationToken>());
    }
}