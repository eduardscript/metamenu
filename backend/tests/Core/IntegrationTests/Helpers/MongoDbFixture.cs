﻿namespace IntegrationTests.Helpers;

public class MongoDbFixture : BaseIntegrationTest
{
    public static List<Tenant> CreatedTenants { get; } = [];
    
    public static void Reset()
    {
        CreatedTenants.Clear();
    }

    public static async Task<Tenant> CreateTenantAsync(Tenant? tenant = default!)
    {
        var tenantRepository = GetService<ITenantRepository>();
        
        tenant ??= Fixture.Create<Tenant>();
        
        tenant.CreatedAt = TimeProvider.GetUtcNow().DateTime;

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