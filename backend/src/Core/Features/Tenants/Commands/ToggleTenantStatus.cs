namespace Core.Features.Tenants.Commands;

public static class ToggleTenantStatus
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository)
        {
            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .GreaterThanZeroAndRequired()
                .MustAsync(async (code, token) => await tenantRepository.ExistsAsync(code, token))
                .WithMessage((c) => CustomValidatorsMessages.EntityNotFoundMessage(nameof(Tenant), nameof(Tenant.Code), c.Code));
        }
    }

    public class Command(int code, bool isEnabled) : IRequest<TenantStatusDto>
    {
        public int Code { get; set; } = code;

        public bool IsEnabled { get; } = isEnabled;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, TenantStatusDto>
    {
        public async Task<TenantStatusDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateDone = await tenantRepository.Update(request.Code, request.IsEnabled, cancellationToken);

            return new TenantStatusDto(
                updateDone);
        }
    }

    public record TenantStatusDto(
        bool StatusUpdated);
}