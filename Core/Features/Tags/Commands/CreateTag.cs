namespace Core.Features.Tags.Commands;

public static class CreateTag
{
    public record Command(
        int TenantCode,
        string TagCategoryCode,
        string TagCode) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository,
        ITagRepository tagRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var tag = new Tag(request.TenantCode, request.TagCategoryCode, request.TagCode);

            if (!await tenantRepository.ExistsByCodeAsync(tag.TenantCode, cancellationToken))
                throw new TenantNotFoundException(tag.TenantCode);

            if (!await tagCategoryRepository.ExistsByAsync(tag.TenantCode, tag.TagCategoryCode, cancellationToken))
                throw new TagCategoryNotFoundException(tag.TagCategoryCode);

            await tagRepository.CreateAsync(tag, cancellationToken);
        }
    }
}