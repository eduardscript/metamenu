using Core.Features.Tags.Commands;

namespace UnitTests.Features.Tags.Commands.RenameTagCodeTests;

[TestClass]
public class Validator : TestBaseValidator<RenameTagCode.Validator, RenameTagCode.Command>
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
    public void OldTagCode_Empty_FailsValidation(string oldTagCode)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.OldTagCode = oldTagCode;
        ExpectedErrorMessage =
            CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.OldTagCode)
                .Humanize(LetterCasing.Title));
    }

    [TestMethod]
    public void OldTagCode_NotFoundTag_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityNotFoundMessage(nameof(Tag), nameof(Tag.Code), Command.OldTagCode);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void NewTagCode_Empty_FailsValidation(string newTagCode)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagRepositoryMock.ExistsAsync(Arg.Any<TagFilter>(), default).Returns(true);

        Command.NewTagCode = newTagCode;
        ExpectedErrorMessage =
            CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.NewTagCode)
                .Humanize(LetterCasing.Title));
    }

    [TestMethod]
    public void NewTagCode_AlreadyExistsTag_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagRepositoryMock.ExistsAsync(Arg.Any<TagFilter>(), default).Returns(true);

        ExpectedErrorMessage =
            CustomValidatorsMessages.EntityAlreadyExistsMessage(nameof(Tag), nameof(Tag.Code), Command.NewTagCode);
    }
}