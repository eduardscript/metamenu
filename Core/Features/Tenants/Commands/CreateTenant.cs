namespace Core.Features.Tenants.Commands;

public static class CreateTenant
{
    public record Command(
        int TenantCode,
        string Name) : IRequest;

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (await tenantRepository.ExistsByCodeAsync(request.TenantCode, cancellationToken))
                throw new TenantAlreadyExistsException(request.TenantCode);

            var tenant = new Tenant(
                request.TenantCode,
                request.Name,
                false);

            await tenantRepository.CreateAsync(tenant, cancellationToken);
        }
    }
}