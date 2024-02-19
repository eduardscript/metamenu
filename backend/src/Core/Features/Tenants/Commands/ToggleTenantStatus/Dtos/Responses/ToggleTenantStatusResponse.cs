namespace Core.Features.Tenants.Commands.ToggleTenantStatus.Dtos.Responses;

public record ToggleTenantStatusResponse(
    bool statusUpdated);