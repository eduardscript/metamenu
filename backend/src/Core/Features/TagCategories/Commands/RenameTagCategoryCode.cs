using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;

namespace Core.Features.TagCategories.Commands;

public static class RenameTagCategoryCode
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.OldTagCategoryCode)
                .ExistsTagCategory(tagCategoryRepository);
            
            RuleFor(c => c.NewTagCategoryCode)
                .AlreadyExistsTagCategory(tagCategoryRepository);
        }
    }

    public class Command(
        int tenantCode,
        string oldTagCategoryCode,
        string newTagCategoryCode) : IRequest
    {
        public int TenantCode { get; set; } = tenantCode;
        
        public string OldTagCategoryCode { get; set; } = oldTagCategoryCode;
        
        public string NewTagCategoryCode { get; set; } = newTagCategoryCode;
    }

    public class Handler(ITagCategoryRepository tagCategoryRepository) : IRequestHandler<Command>
    {
        public Task Handle(Command request, CancellationToken cancellationToken)
        {
            return tagCategoryRepository.RenameAsync(request.TenantCode, request.OldTagCategoryCode, request.NewTagCategoryCode, cancellationToken);
        }
    }
}