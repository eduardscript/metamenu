using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands.CreateTagTests;

[TestClass]
public class Validator : TestBaseValidator<CreateTag.Validator, CreateTag.Command>
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
    public void TagCategoryCode_Empty_FailsValidation(string tagCategoryCode)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.TagCategoryCode = tagCategoryCode;
        ExpectedErrorMessage =
            CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.TagCategoryCode)
                .Humanize(LetterCasing.Title));
    }

    [TestMethod]
    public void TagCategoryCode_NotFoundTagCategory_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.Code, default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityNotFoundMessage(nameof(TagCategory).Humanize(LetterCasing.Title), nameof(TagCategory.Code), Command.TagCategoryCode);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Code_Empty_FailsValidation(string code)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.TagCategoryCode, default).Returns(true);

        Command.Code = code;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.Code));
    }

    [TestMethod]
    public void Code_AlreadyExistsTagCategory_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagCategoryRepositoryMock.ExistsAsync(Command.TenantCode, Command.TagCategoryCode, default).Returns(true);
        TagRepositoryMock.ExistsAsync(Arg.Any<TagFilter>(), default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityAlreadyExistsMessage(nameof(Tag), nameof(Tag.Code), Command.Code);
    }
}