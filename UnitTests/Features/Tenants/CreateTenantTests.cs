using Core.Features.Tenants.Commands;

namespace UnitTests.Features.Tenants;

[TestClass]
public class CreateTenantTests : TestBase
{
    private readonly Tenant _tenant = Fixture.Create<Tenant>();

    [TestMethod]
    public async Task Handle_WhenTenantExists_ThrowsTenantAlreadyExistsException()
    {
        // Arrange
        var tenantRepository = Substitute.For<ITenantRepository>();
        tenantRepository.ExistsByCodeAsync(_tenant.TenantCode, default).Returns(true);

        var handler = new CreateTenant.Handler(tenantRepository);

        var action = new Func<Task>(async () =>
            await handler.Handle(new CreateTenant.Command(_tenant.TenantCode, _tenant.Name), default));

        // Act and Assert
        await action.Should().ThrowAsync<TenantAlreadyExistsException>();

        await tenantRepository.Received().ExistsByCodeAsync(_tenant.TenantCode, default);
        await tenantRepository.DidNotReceive().CreateAsync(_tenant, default);
    }
}