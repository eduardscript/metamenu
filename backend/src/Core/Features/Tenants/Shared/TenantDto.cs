namespace Core.Features.Tenants.Shared;

public class TenantDto()
{
    public int Code { get; set; }

    public string Name { get; set; } = null!;

    public string DefaultTagCategory { get; set; } = null!;

    public int Template { get; set; }

    public bool IsEnabled { get; set; } = false;

    public Address Address { get; set; } = new();

    public IEnumerable<WeekDay> Weekdays { get; set; } = Array.Empty<WeekDay>();

    public DateTime CreatedAt { get; set; }
};

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

public static class TenantDtoExtensions
{
    public static IEnumerable<TenantDto> ToDto(this IEnumerable<Tenant> tenants)
    {
        return tenants.Select(x => x.ToDto());
    }

    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto()
        {
            Code = tenant.Code,
            Name = tenant.Name,
            DefaultTagCategory = tenant.DefaultTagCategory,
            IsEnabled = tenant.IsEnabled,
            CreatedAt = tenant.CreatedAt,
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