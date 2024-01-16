namespace Core.Features.Tenants.Commands;

public static class DeleteTenant
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository)
        {
            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .NotEmptyAndRequired()
                .MustAsync(async (code, token) => await tenantRepository.ExistsAsync(code, token))
                .WithMessage((c) => CustomValidatorsMessages.EntityNotFoundMessage(nameof(Tenant), nameof(Tenant.Code), c.Code));
        }
    }

    public class Command(int code) : IRequest<TenantDeletedDto>
    {
        public int Code { get; set; } = code;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, TenantDeletedDto>
    {
        public async Task<TenantDeletedDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenantWasDeleted = await tenantRepository.DeleteAsync(request.Code, cancellationToken);

            return new TenantDeletedDto(
                tenantWasDeleted);
        }
    }

    public record TenantDeletedDto(
        bool IsDeleted);
}