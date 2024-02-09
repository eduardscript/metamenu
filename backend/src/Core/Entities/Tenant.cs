using Core.Features.Tenants.Commands;

namespace Core.Entities;

public class Tenant(
    string name)
{
    public int Code { get; set; }

    public string Name { get; private set; } = name;

    public string DefaultTagCategory { get; set; } = null!;

    public int Template { get; set; }

    public bool IsEnabled { get; set; } = false;

    public Address Address { get; set; } = new();
    public IEnumerable<WeekDay> Weekdays { get; set; } = Array.Empty<WeekDay>();

    public DateTime CreatedAt { get; set; }
}

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

public class Address
{
    public string Street { get; set; } = null!;
    public int Number { get; set; }
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}

public static class TenantExtensions
{
    public static Tenant ToEntity(this CreateTenant.Command tenant, DateTime? createdAt)
    {
        return new Tenant(tenant.Name)
        {
            IsEnabled = tenant.IsEnabled,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            DefaultTagCategory = tenant.DefaultTagCategory,
            Template = 1,
            Address = new Address()
            {
                City = tenant.Address.City,
                Country = tenant.Address.Country,
                Number = tenant.Address.Number,
                Street = tenant.Address.Street
            },
            Weekdays = tenant.Weekdays.Select(wd => new WeekDay()
            {
                Name = wd.Name,
                Schedules = wd.Schedules.Select(sc => new Schedule()
                {
                    Start = sc.Start,
                    End = sc.End
                })
            })
        };
    }
}