namespace Core.Entities;

public record User(
    string Username,
    string Password,
    IEnumerable<int> AvailableTenants)
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}