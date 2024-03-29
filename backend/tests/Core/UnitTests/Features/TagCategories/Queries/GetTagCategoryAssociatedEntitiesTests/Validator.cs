﻿using Core.Exceptions.TagCategories;
using Core.Features.TagCategories.Queries;

namespace UnitTests.Features.TagCategories.Queries.GetTagCategoryAssociatedEntitiesTests;

[TestClass]
public class Validator : TestBaseValidator<GetTagCategoryAssociatedEntities.Validator, GetTagCategoryAssociatedEntities.Query>
{
    [TestMethod]
    public void InvalidTenantCode_ThrowsValidationException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);
        
        TagCategoryRepositoryMock.GetByAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns(Fixture.Create<TagCategory>());

        Command.TenantCode = 0;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.TenantCode).Humanize(LetterCasing.Title));
    }
    
    [TestMethod]
    public void InvalidTagCategoryCode_ThrowsValidationException()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Arg.Any<int>(), Arg.Any<CancellationToken>())
            .Returns(true);
        
        TagCategoryRepositoryMock.GetByAsync(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<CancellationToken>())
            .Returns((TagCategory)null!);
        
        ExpectedErrorMessage = new TagCategoryNotFoundException(Command.TagCategoryCode).Message;
    }
}