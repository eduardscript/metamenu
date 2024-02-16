namespace Core.Features.TagCategories.Commands;

public static class DeleteTagCategory
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.TagCategoryCode)
                .ExistsTagCategory(tagCategoryRepository);
        }
    }

    public class Command(
        int tenantCode,
        string tagCategoryCode) : IRequest<DeleteTagCategoryDto>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string TagCategoryCode { get; set; } = tagCategoryCode;
    }

    public class Handler(
        ITagCategoryRepository tagCategoryRepository,
        ITagRepository tagRepository) : IRequestHandler<Command, DeleteTagCategoryDto>
    {
        public async Task<DeleteTagCategoryDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await tagCategoryRepository.DeleteAsync(
                request.TenantCode,
                request.TagCategoryCode,
                cancellationToken);

            await tagRepository.DeleteManyAsync(new(request.TenantCode)
            {
                TagCategoryCode = request.TagCategoryCode
            }, cancellationToken);

            return new(result);
        }
    }

    public record DeleteTagCategoryDto(
        bool IsDeleted);
}