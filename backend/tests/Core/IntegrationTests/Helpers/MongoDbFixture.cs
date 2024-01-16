namespace IntegrationTests.Helpers;

public class MongoDbFixture : IntegrationTestBase
{
    public static List<Tenant> CreatedTenants { get; } = new();
    
    public static void Reset()
    {
        CreatedTenants.Clear();
    }

    public static async Task<Tenant> CreateTenantAsync()
    {
        var tenantRepository = GetService<ITenantRepository>();

        var tenant = Fixture.Create<Tenant>();
        var tenantCode = (await tenantRepository.CreateAsync(tenant, default)).Code;

        tenant.Code = tenantCode;

        CreatedTenants.Add(tenant);

        return tenant;
    }
    
    public static async Task CreateTenantsAsync(int count = 3)
    {
        var tenantTasks = Enumerable
            .Range(0, count)
            .Select(_ => CreateTenantAsync());

        await Task.WhenAll(tenantTasks);
    }
}