namespace Core.Features.TagCategories.Commands;

public static class RenameTagCategoryCode
{
    public record Command(
        int TenantCode,
        string OldTagCategoryCodeName,
        string NewTagCategoryCodeName) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await tenantRepository.ExistsByCodeAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }

            if (await tagCategoryRepository.ExistsByAsync(request.TenantCode, request.NewTagCategoryCodeName, cancellationToken))
            {
                throw new TagCategoryAlreadyExistsException(request.OldTagCategoryCodeName);
            }

            await tagCategoryRepository.RenameAsync(request.TenantCode, request.OldTagCategoryCodeName, request.NewTagCategoryCodeName, cancellationToken);
        }
    }
}