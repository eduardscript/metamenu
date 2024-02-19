using Core.Features.Tenants.Commands.ToggleTenantStatus.Dtos.Responses;

namespace Core.Features.Tenants.Commands.ToggleTenantStatus;

public static class ToggleTenantStatus
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository)
        {
            RuleFor(x => x.Code)
                .Cascade(CascadeMode.Stop)
                .ExistsTenant(tenantRepository);
        }
    }

    public class Command(int code, bool isEnabled) : IRequest<ToggleTenantStatusResponse>
    {
        public int Code { get; set; } = code;

        public bool IsEnabled { get; } = isEnabled;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, ToggleTenantStatusResponse>
    {
        public async Task<ToggleTenantStatusResponse> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateDone = await tenantRepository.UpdateAsync(request.Code,
                new UpdateTenantFilter(isEnabled: request.IsEnabled), cancellationToken);

            return new ToggleTenantStatusResponse(
                updateDone);
        }
    }
}