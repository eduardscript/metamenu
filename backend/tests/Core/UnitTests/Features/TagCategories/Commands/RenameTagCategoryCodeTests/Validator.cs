using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands.RenameTagCategoryCodeTests;

[TestClass]
public class Validator : TestBaseValidator<RenameTagCategoryCode.Validator, RenameTagCategoryCode.Command>
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void TenantCode_LessThanZero_FailsValidation(int code)
    {
        // Arrange
        Command.TenantCode = code;
        ExpectedErrorMessage =
            CustomValidatorsMessages.GreaterThanZeroAndRequiredMessage(nameof(Command.TenantCode)
                .Humanize(LetterCasing.Title));
    }

    [TestMethod]
    public void TenantCode_NotExistingTenant_FailsValidation()
    {
        // Arrange
        Command.TenantCode = 1;
        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityNotFoundMessage(nameof(Tenant), nameof(Tenant.Code), Command.TenantCode);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void OldTagCategoryCode_Empty_FailsValidation(string oldTagCategoryCode)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.OldTagCategoryCode = oldTagCategoryCode;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.OldTagCategoryCode).Humanize(LetterCasing.Title));
    }
    
    [TestMethod]
    public void OldTagCategoryCode_NotExistingTagCategory_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityNotFoundMessage(nameof(TagCategory), nameof(TagCategory.Code), Command.OldTagCategoryCode);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void NewTagCategoryCode_Empty_FailsValidation(string newTagCategoryCode)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.OldTagCategoryCode, default).Returns(true);

        Command.NewTagCategoryCode = newTagCategoryCode;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.NewTagCategoryCode).Humanize(LetterCasing.Title));
    }
    
    [TestMethod]
    public void NewTagCategoryCode_AlreadyExistingTagCategory_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.OldTagCategoryCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.NewTagCategoryCode, default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityAlreadyExistsMessage(nameof(TagCategory), nameof(TagCategory.Code), Command.NewTagCategoryCode);
    }
}