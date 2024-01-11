using Core.Features.Tenants.Shared;

namespace Core.Features.Tenants.Commands;

public static class CreateTenant
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }

    public class Command(string name) : IRequest<TenantDto>
    {
        public string Name { get; set; } = name;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, TenantDto>
    {
        public async Task<TenantDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenant = new Tenant(
                request.Name,
                false);

            var newTenant = await tenantRepository.CreateAsync(tenant, cancellationToken);

            return newTenant.ToDto();
        }
    }
}