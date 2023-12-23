namespace Core.Exceptions.Tenants;

public class TenantNotFoundException(int tenantCode) : Exception($"Tenant with code {tenantCode} was not found.");