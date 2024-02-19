using Core.Features.Tenants.Queries.Dtos.Responses;

namespace Core.Features.Tenants.Queries.Extensions;

public static class TenantQueryExtensions
{
    public static GetAllTenantsResponse ToDto(this IEnumerable<Tenant> tenants)
    {
        return new GetAllTenantsResponse(tenants.Select(x => x.ToDto()));
    }

    public static TenantDto ToDto(this Tenant tenant)
    {
        return new TenantDto(
            tenant.Code,
            tenant.Name,
            tenant.DefaultTagCategory,
            tenant.IsEnabled,
            tenant.Template,
            tenant.Address?.ToDto(),
            tenant.Weekdays?.ToDto());
    }

    public static AddressDto ToDto(this Address address)
    {
        return new AddressDto(address.Street, address.Number, address.City, address.Country);
    }

    public static IEnumerable<WeekDayDto> ToDto(this IEnumerable<WeekDay> weekDays)
    {
        var result = new List<WeekDayDto>();

        foreach (var weekDay in weekDays)
        {
            var weekDaySchedules = new List<ScheduleDto>();

            foreach (var schedule in weekDay.Schedules)
            {
                weekDaySchedules.Add(new ScheduleDto(schedule.Start, schedule.End));
            }

            result.Add(new WeekDayDto(weekDay.Name, weekDaySchedules));
        }

        return result;
    }
}