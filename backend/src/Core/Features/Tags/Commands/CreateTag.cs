namespace Core.Features.Tags.Commands;

public static class CreateTag
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository,
            ITagRepository tagRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.TagCategoryCode)
                .ExistsTagCategory(tagCategoryRepository);

            RuleFor(c => c.Code)
                .AlreadyExistsTag(tagRepository);
        }
    }

    public class Command(
        int tenantCode,
        string tagCategoryCode,
        string code) : IRequest
    {
        public int TenantCode { get; set; } = tenantCode;

        public string TagCategoryCode { get; set; } = tagCategoryCode;

        public string Code { get; set; } = code;
    }

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var tag = new Tag(request.TenantCode, request.TagCategoryCode, request.Code);

            await tagRepository.CreateAsync(tag, cancellationToken);
        }
    }
}

public static class DeleteTag
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagRepository tagRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);

            RuleFor(c => c.Code)
                .ExistsTag(tagRepository);
        }
    }

    public class Command(
        int tenantCode,
        string tagCategoryCode,
        string code) : IRequest<TagDeletedDto>
    {
        public int TenantCode { get; set; } = tenantCode;

        public string TagCategoryCode { get; set; } = tagCategoryCode;

        public string Code { get; set; } = code;
    }

    public class Handler(ITagRepository tagRepository) : IRequestHandler<Command, TagDeletedDto>
    {
        public async Task<TagDeletedDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tagWasDeleted = await tagRepository.DeleteAsync(request.TenantCode, request.TagCategoryCode, request.Code, cancellationToken);
            
            return new TagDeletedDto(tagWasDeleted);
        }
    }

    public record TagDeletedDto(
        bool IsDeleted);
}