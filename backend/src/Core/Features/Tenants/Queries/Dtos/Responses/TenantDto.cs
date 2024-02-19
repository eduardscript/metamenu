namespace Core.Features.Tenants.Queries.Dtos.Responses;

public class TenantDto(
    int code,
    string name,
    string? defaultTagCategory,
    bool isEnabled,
    int? template,
    AddressDto? address,
    IEnumerable<WeekDayDto>? weekDays)
{
    public int Code { get; set; } = code;

    public string Name { get; private set; } = name;

    public string? DefaultTagCategory { get; set; } = defaultTagCategory;

    public bool IsEnabled { get; set; } = isEnabled;

    public int? Template { get; set; } = template;

    public AddressDto? Address { get; set; } = address;

    public IEnumerable<WeekDayDto>? Weekdays { get; set; } = weekDays;
}

public class WeekDayDto(string name, IEnumerable<ScheduleDto> schedules)
{
    public string Name { get; set; } = name;

    public IEnumerable<ScheduleDto> Schedules { get; set; } = schedules;
}

public class ScheduleDto(string start, string end)
{
    public string Start { get; set; } = start;
    public string End { get; set; } = end;
}

public class AddressDto(string street, int number, string city, string country)
{
    public string Street { get; set; } = street;
    public int Number { get; set; } = number;
    public string City { get; set; } = city;
    public string Country { get; set; } = country;
}