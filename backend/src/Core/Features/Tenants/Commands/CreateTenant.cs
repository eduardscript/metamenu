using Core.Features.Tenants.Shared;

namespace Core.Features.Tenants.Commands;

public static class CreateTenant
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITagCategoryRepository tagCategoryRepository)
        {
            RuleFor(x => x.Name)
                .NotEmptyAndRequired();
        }
    }

    public class Command() : IRequest<TenantDto>
    {
        public string Name { get; set; } = null!;

        public string DefaultTagCategory { get; set; } = null!;

        public int Template { get; set; }

        public bool IsEnabled { get; private set; } = false;

        public Shared.Address Address { get; set; } = new();

        public IEnumerable<Shared.WeekDay> Weekdays { get; set; } = Array.Empty<Shared.WeekDay>();
    }

    public class Handler(ITenantRepository tenantRepository, TimeProvider timeProvider)
        : IRequestHandler<Command, TenantDto>
    {
        public async Task<TenantDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenant = request.ToEntity(timeProvider.GetUtcNow().DateTime);
            var newTenant = await tenantRepository.CreateAsync(tenant, cancellationToken);

            return newTenant.ToDto();
        }
    }
}