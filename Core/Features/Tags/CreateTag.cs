namespace Core.Features.Tags;

public static class CreateTag
{
    public record Command(
        int TenantCode,
        string Code,
        string TagCategoryCode) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository,
        ITagRepository tagRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await tenantRepository.ExistsByCodeAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }

            if (!await tagCategoryRepository.ExistsByCodeAsync(request.TagCategoryCode, cancellationToken))
            {
                throw new TagCategoryNotFoundException(request.TagCategoryCode);
            }

            var tag = new Tag(
                request.TenantCode,
                request.Code,
                request.TagCategoryCode);

            await tagRepository.CreateAsync(tag, cancellationToken);
        }
    }
}
