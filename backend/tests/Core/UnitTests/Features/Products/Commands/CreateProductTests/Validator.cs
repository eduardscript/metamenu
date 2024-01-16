using Core.Features.Products.Commands;

namespace UnitTests.Features.Products.Commands.CreateProductTests;

[TestClass]
public class Validator : TestBaseValidator<CreateProduct.Validator, CreateProduct.Command>
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
    public void TagCodes_Empty_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.TagCodes = new List<string>();

        ExpectedErrorMessage =
            CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.TagCodes).Humanize(LetterCasing.Title));
    }

    [TestMethod]
    public void TagCodes_Duplicated_FailsValidation()
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.TagCodes = new[]
        {
            "TagCodeInvalid",
            "TagCodeInvalid",
            "TagCode",
            "TagCode",
        };

        ExpectedErrorMessage = CustomValidatorsMessages.UniqueMessage(
            nameof(Command.TagCodes).Humanize(LetterCasing.Title), 
            string.Join(", ", Command.TagCodes.Distinct()));
    }

    [TestMethod]
    public void TagCodes_InvalidTags_FailsValidation()
    {
        // Arrange
        var tags = Fixture.Build<Tag>()
            .With(t => t.TenantCode, Command.TenantCode)
            .CreateMany()
            .ToList();

        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);
        TagRepositoryMock.GetAll(Arg.Is<ITagRepository.TagFilter>(f => f.TenantCode == Command.TenantCode), default)
            .Returns(tags);

        Command.TagCodes = new List<string>(tags.Select(t => t.Code))
        {
            "InvalidTagCode1",
            "InvalidTagCode2"
        };

        ExpectedErrorMessage =
            $"The following tag codes are invalid: {string.Join(", ", Command.TagCodes.Skip(tags.Count))}";
    }
    
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Name_Empty_FailsValidation(string name)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.Name = name;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.Name));
    }
    
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Price_LessThanZero_FailsValidation(double price)
    {
        // Arrange
        TenantRepositoryMock.ExistsAsync(Command.TenantCode, default).Returns(true);

        Command.Price = (decimal)price;
        ExpectedErrorMessage = CustomValidatorsMessages.GreaterThanZeroAndRequiredMessage(nameof(Command.Price));
    }
}