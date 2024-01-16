using Core.Features.TagCategories.Commands;

namespace UnitTests.Features.TagCategories.Commands.CreateTagCategoryTests;

[TestClass]
public class Validator : TestBaseValidator<CreateTagCategory.Validator, CreateTagCategory.Command>
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
    public void Code_Empty_FailsValidation(string code)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.Code = code;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.Code));
    }

    [TestMethod]
    public void Code_AlreadyExistsTagCategory_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.Code, default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityAlreadyExistsMessage(nameof(TagCategory), nameof(TagCategory.Code), Command.Code);
    }
}