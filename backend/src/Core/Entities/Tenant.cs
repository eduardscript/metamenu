namespace Core.Entities;

public class Tenant(
    string name)
{
    public int Code { get; set; }

    public string Name { get; private set; } = name;

    public string? DefaultTagCategory { get; set; }

    public bool IsEnabled { get; set; } = false;

    public int? Template { get; set; }

    public Address? Address { get; set; }

    public IEnumerable<WeekDay>? Weekdays { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class WeekDay(string name, IEnumerable<Schedule> schedules)
{
    public string Name { get; set; } = name;

    public IEnumerable<Schedule> Schedules { get; set; } = schedules;
}

public class Schedule(string start, string end)
{
    public string Start { get; set; } = start;
    public string End { get; set; } = end;
}

public class Address(string street, int number, string city, string country)
{
    public string Street { get; set; } = street;
    public int Number { get; set; } = number;
    public string City { get; set; } = city;
    public string Country { get; set; } = country;
}