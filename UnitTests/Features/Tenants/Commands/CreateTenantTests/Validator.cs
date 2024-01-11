using Core.Features.Tenants.Commands;

namespace UnitTests.Features.Tenants.Commands.CreateTenantTests;

[TestClass]
public class Validator : TestBaseValidator<CreateTenant.Validator, CreateTenant.Command>
{
    [TestMethod]
    [DataRow("")]
    [DataRow(" ")]
    [DataRow(null)]
    public void Validate_EmptyPassword_FailsValidation(string name)
    {
        // Arrange
        Command.Name = name;
        ExpectedErrorMessage = "'Name' must not be empty.";
    }
}