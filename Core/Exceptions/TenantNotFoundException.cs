namespace Core.Exceptions;

public class TenantNotFoundException(int tenantCode) : Exception($"Tenant with code {tenantCode} was not found.");