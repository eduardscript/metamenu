namespace Core.Features.Tenants.Commands;

public static class ToggleTenantStatus
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository)
        {
            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .NotEmptyAndRequired()
                .MustAsync(async (code, token) => await tenantRepository.ExistsAsync(code, token));
        }
    }

    public class Command(int code, bool isEnabled) : IRequest<TenantStatusDto>
    {
        public int Code { get; set; } = code;
        
        public bool IsEnabled { get; set; } = isEnabled;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, TenantStatusDto>
    {
        public async Task<TenantStatusDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var updatedTenantStatus = await tenantRepository.Update(request.Code, request.IsEnabled, cancellationToken);

            return new TenantStatusDto(
                updatedTenantStatus);
        }
    }

    public record TenantStatusDto(
        bool IsEnabled);
}