namespace Core.Features.Tags.Commands;

public static class RenameTagCode
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagRepository tagRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);
            
            RuleFor(c => c.OldTagCode)
                .ExistsTag(tagRepository);

            RuleFor(c => c.NewTagCode)
                .AlreadyExistsTag(tagRepository);
        }
    }

    public class Command(
        int tenantCode,
        string oldTagCode,
        string newTagCode) : IRequest
    {
        public int TenantCode { get; set; } = tenantCode;
        
        public string OldTagCode { get; set; } = oldTagCode;
        
        public string NewTagCode { get; set; } = newTagCode;
    }

    public class Handler(
        ITenantRepository tenantRepository,
        ITagRepository tagRepository) : IRequestHandler<Command>
    {
        public Task Handle(Command request, CancellationToken cancellationToken)
        {
            return tagRepository.RenameAsync(request.TenantCode, request.OldTagCode, request.NewTagCode, cancellationToken);
        }
    }
}