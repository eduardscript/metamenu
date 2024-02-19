using Core.Features.Tenants.Commands.ConfigureTenantCommand.Dtos.Responses;
using Requests = Core.Features.Tenants.Commands.ConfigureTenantCommand.Dtos.Requests;

namespace Core.Features.Tenants.Commands.ConfigureTenantCommand;

public static class ConfigureTenant
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository)
        {
            RuleFor(x => x.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.DefaultTagCategory)
                .ExistsTagCategory(tagCategoryRepository);
        }
    }

    public class Command(
        int tenantCode,
        string defaultTagCategory,
        int template,
        Requests.Address address,
        IEnumerable<Requests.WeekDay> weekDays) : IRequest<ConfigureTenantResponse>
    {
        public int TenantCode { get; set; } = tenantCode;
        public string DefaultTagCategory { get; set; } = defaultTagCategory;

        public int Template { get; set; } = template;

        public Requests.Address Address { get; set; } = address;

        public IEnumerable<Requests.WeekDay> WeekDays { get; set; } = weekDays;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, ConfigureTenantResponse>
    {
        public async Task<ConfigureTenantResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var fitler = new UpdateTenantFilter(null, null, request.DefaultTagCategory, request.Template,
                request.Address, request.WeekDays);

            await tenantRepository.UpdateAsync(request.TenantCode, fitler, cancellationToken);

            return new ConfigureTenantResponse(true);
        }
    }
}