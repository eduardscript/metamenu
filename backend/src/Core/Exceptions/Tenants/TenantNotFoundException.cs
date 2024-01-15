namespace Core.Exceptions.Tenants;

public class TenantNotFoundException(int tenantCode) : Exception($"Tenant with code {tenantCode} was not found.");

public class TenantsNotFoundException(IEnumerable<int> tenantCodes) : Exception($"Tenants with codes {string.Join(",", tenantCodes)} were not found.");
