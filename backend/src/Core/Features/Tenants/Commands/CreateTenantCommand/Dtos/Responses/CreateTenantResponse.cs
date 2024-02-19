namespace Core.Features.Tenants.Commands.CreateTenantCommand.Dtos.Responses;

public record CreateTenantResponse(
    int Code,
    string Name,
    bool IsEnabled,
    DateTime CreatedAt);