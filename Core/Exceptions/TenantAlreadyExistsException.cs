namespace Core.Exceptions;

public class TenantAlreadyExistsException(int tenantCode) : Exception($"Tenant with code {tenantCode} already exists.");