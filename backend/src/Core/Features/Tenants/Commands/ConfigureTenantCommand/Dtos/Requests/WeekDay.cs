namespace Core.Features.Tenants.Commands.ConfigureTenantCommand.Dtos.Requests;

public class WeekDay
{
    public string Name { get; set; } = null!;

    public IEnumerable<Schedule> Schedules { get; set; } = Array.Empty<Schedule>();
}

public class Schedule
{
    public string Start { get; set; } = null!;
    public string End { get; set; } = null!;
}