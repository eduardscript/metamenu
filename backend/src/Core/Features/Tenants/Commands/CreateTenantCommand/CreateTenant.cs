using Core.Features.Tenants.Commands.CreateTenantCommand.Dtos.Responses;
using Core.Features.Tenants.Commands.CreateTenantCommand.Extensions;

namespace Core.Features.Tenants.Commands.CreateTenantCommand;

public static class CreateTenant
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Name)
                .NotEmptyAndRequired();
        }
    }

    public class Command(string name) : IRequest<CreateTenantResponse>
    {
        public string Name { get; set; } = name;
    }

    public class Handler(ITenantRepository tenantRepository, TimeProvider timeProvider) : IRequestHandler<Command, CreateTenantResponse>
    {
        public async Task<CreateTenantResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenant = new Tenant(request.Name)
            {
                CreatedAt = timeProvider.GetUtcNow().DateTime
            };

            var newTenant = await tenantRepository.CreateAsync(tenant, cancellationToken);

            return newTenant.ToDto();
        }
    }
}