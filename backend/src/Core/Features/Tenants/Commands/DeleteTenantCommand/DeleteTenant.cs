using Core.Features.Tenants.Commands.DeleteTenantCommand.Dtos.Responses;

namespace Core.Features.Tenants.Commands.DeleteTenantCommand;

public static class DeleteTenant
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

    public class Command(int code) : IRequest<DeleteTenantDto>
    {
        public int Code { get; set; } = code;
    }

    public class Handler(ITenantRepository tenantRepository) : IRequestHandler<Command, DeleteTenantDto>
    {
        public async Task<DeleteTenantDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenantWasDeleted = await tenantRepository.DeleteAsync(request.Code, cancellationToken);

            return new DeleteTenantDto(
                tenantWasDeleted);
        }
    }
}