namespace Core.Features.Tenants.Commands.ConfigureTenantCommand.Dtos.Requests;

public class Address
{
    public string Street { get; set; } = null!;
    public int Number { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}