namespace Core.Entities;

public record Tenant(
    string Name,
    bool IsEnabled = false)
{
    public int Code { get; init; }
}