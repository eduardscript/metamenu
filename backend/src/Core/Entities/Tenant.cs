namespace Core.Entities;

public class Tenant(
    string name)
{
    public int Code { get; set; }

    public string Name { get; private set; } = name;

    public bool IsEnabled { get; set; } = false;

    public DateTime CreatedAt { get; set; }
}