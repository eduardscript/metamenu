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
        string newTagCategoryCode) : IRequest<RenameTagCategoryDto>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string OldTagCategoryCode { get; set; } = oldTagCategoryCode;

        public string NewTagCategoryCode { get; set; } = newTagCategoryCode;
    }

    public class Handler(
        ITagCategoryRepository tagCategoryRepository,
        ITagRepository tagRepository) : IRequestHandler<Command, RenameTagCategoryDto>
    {
        public async Task<RenameTagCategoryDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await tagCategoryRepository.RenameAsync(
                request.TenantCode,
                request.OldTagCategoryCode,
                request.NewTagCategoryCode,
                cancellationToken);

            await tagRepository.UpdateManyAsync(
                new(request.TenantCode) { TagCategoryCode = request.OldTagCategoryCode },
                new() { NewTagCategoryCode = request.NewTagCategoryCode },
                cancellationToken);

            return new(result);
        }
    }

    public record RenameTagCategoryDto(
        bool IsUpdated);
}