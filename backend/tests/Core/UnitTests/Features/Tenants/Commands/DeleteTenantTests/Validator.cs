﻿using Core.Features.Tenants.Commands;

namespace UnitTests.Features.Tenants.Commands.DeleteTenantTests;

[TestClass]
public class Validator : TestBaseValidator<DeleteTenant.Validator, DeleteTenant.Command>
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