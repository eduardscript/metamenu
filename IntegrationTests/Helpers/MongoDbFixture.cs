namespace IntegrationTests.Helpers;

public class MongoDbFixture : IntegrationTestBase {
    public static List<Tenant> CreatedTenants { get; } = new();

    public static async Task<Tenant> CreateTenantAsync()
    {
        var tenantRepository = GetService<ITenantRepository>();

        var tenant = Fixture.Create<Tenant>();
        var tenantCode = (await tenantRepository.CreateAsync(tenant, default)).TenantCode;

        tenant = tenant with { TenantCode = tenantCode };

        CreatedTenants.Add(tenant);

        return tenant;
    }
    
    public static async Task<IEnumerable<Tenant>> CreateTenantsAsync(int count = 3)
    {
        for (var i = 0; i < count; i++)
        {
            await CreateTenantAsync();
        }
       
        return CreatedTenants;
    }
}