using Core.Exceptions.TagCategories;
using Core.Features.TagCategories.Shared;

namespace Core.Features.TagCategories.Commands;

public static class CreateTagCategory
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ValidTenant(tenantRepository);

            RuleFor(c => c.Code)
                .AlreadyExistsTagCategory(tagCategoryRepository);
        }
    }

    public class Command(
        int tenantCode,
        string code) : IRequest<TagCategoryDto>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string Code { get; set; } = code;
    }

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository) : IRequestHandler<Command, TagCategoryDto>
    {
        public async Task<TagCategoryDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tagCategory = new TagCategory(request.TenantCode, request.Code);

            await tagCategoryRepository.CreateAsync(tagCategory, cancellationToken);

            return tagCategory.ToDto();
        }
    }
}