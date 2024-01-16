﻿using Core.Features.Tenants.Commands;

namespace UnitTests.Features.Tenants.Commands.ToggleTenantStatusTests;

[TestClass]
public class Validator : TestBaseValidator<ToggleTenantStatus.Validator, ToggleTenantStatus.Command>
{
    [TestMethod]
    public void EmptyName_FailsValidation()
    {
        // Arrange
        Command.Code = 0;
        ExpectedErrorMessage = CustomValidatorsMessages.NotEmptyAndRequiredMessage(nameof(Command.Code));
    }
    
    [TestMethod]
    public void NotExistingTenant_FailsValidation()
    {
        // Arrange
        Command.Code = 1;
        ExpectedErrorMessage = CustomValidatorsMessages.EntityNotFoundMessage(nameof(Tenant), nameof(Tenant.Code), Command.Code);
    }
}