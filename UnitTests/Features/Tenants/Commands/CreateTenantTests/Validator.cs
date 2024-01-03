using Core.Features.Tenants.Commands;

namespace UnitTests.Features.Tenants.Commands.CreateTenantTests;

[TestClass]
public class Validator : TestBaseValidator<CreateTenant.Validator, CreateTenant.Command>
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void Validate_EmptyTenantCode_FailsValidation(int tenantCode)
    {
        // Arrange
        Command.TenantCode = tenantCode;
        ExpectedErrorMessage = "'Tenant Code' must be greater than '0'.";
    }

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