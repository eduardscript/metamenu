namespace Core.Features.Tags.Commands;

public static class UpdateTag
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(
            ITenantRepository tenantRepository,
            ITagCategoryRepository tagCategoryRepository,
            ITagRepository tagRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.Code)
                .ExistsTag(tagRepository);

            RuleFor(c => c.NewTagCategoryCode)!
                .ExistsTagCategory(tagCategoryRepository)
                .When(c => c.NewTagCategoryCode != null);
             
            RuleFor(c => c.NewTagCode)!
                .AlreadyExistsTag(tagRepository)
                .When(t => t.NewTagCode != null);
        }
    }

    public class Command(
        int tenantCode,
        string code) : IRequest<TagUpdatedDto>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string Code { get; set; } = code;

        public string? NewTagCode { get; set; }

        public string? NewTagCategoryCode { get; set; }
    }

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Command, TagUpdatedDto>
    {
        public async Task<TagUpdatedDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var result = await tagRepository.UpdateAsync(
                new(request.TenantCode)
                {
                    Code = request.Code,
                },
                new()
                {
                    NewTagCode = request.NewTagCode,
                    NewTagCategoryCode = request.NewTagCategoryCode
                },
                cancellationToken);

            return new(result);
        }
    }

    public record TagUpdatedDto(
        bool IsUpdated);
}