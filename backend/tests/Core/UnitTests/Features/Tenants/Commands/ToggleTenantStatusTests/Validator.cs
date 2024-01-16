using Core.Features.Tenants.Commands;

namespace UnitTests.Features.Tenants.Commands.ToggleTenantStatusTests;

[TestClass]
public class Validator : TestBaseValidator<ToggleTenantStatus.Validator, ToggleTenantStatus.Command>
{
    [TestMethod]
    [DataRow(0)]
    [DataRow(-1)]
    public void CodeLessThanZero_FailsValidation(int code)
    {
        // Arrange
        Command.Code = code;
        ExpectedErrorMessage = CustomValidatorsMessages.GreaterThanZeroAndRequiredMessage(nameof(Command.Code));
    }
    
    [TestMethod]
    public void NotExistingTenant_FailsValidation()
    {
        // Arrange
        Command.Code = 1;
        ExpectedErrorMessage = CustomValidatorsMessages.EntityNotFoundMessage(nameof(Tenant), nameof(Tenant.Code), Command.Code);
    }
}