namespace Core.Features.Tags.Commands;

public static class DeleteTag
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator(ITenantRepository tenantRepository, ITagCategoryRepository tagCategoryRepository, ITagRepository tagRepository)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(c => c.TenantCode)
                .ExistsTenant(tenantRepository);
            
            RuleFor(c => c.TagCategoryCode)
                .ExistsTagCategory(tagCategoryRepository);

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

    public class Handler(
        ITagRepository tagRepository,
        IProductRepository productRepository) : IRequestHandler<Command, TagDeletedDto>
    {
        public async Task<TagDeletedDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tagWasDeleted = await tagRepository.DeleteAsync(request.TenantCode, request.TagCategoryCode, request.Code, cancellationToken);
            
            if (tagWasDeleted)
            {
                await productRepository.UpdateManyAsync(
                    new(request.TenantCode) { TagCode = request.Code },
                    new()
                    {
                        TagCategoryCodeToRemove = request.TagCategoryCode
                    },
                    cancellationToken);
            }
            
            return new TagDeletedDto(tagWasDeleted);
        }
    }

    public record TagDeletedDto(
        bool IsDeleted);
}