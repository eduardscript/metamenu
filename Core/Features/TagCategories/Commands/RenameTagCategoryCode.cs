using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;

namespace Core.Features.TagCategories.Commands;

public static class RenameTagCategoryCode
{
    public record Command(
        int TenantCode,
        string OldTagCategoryCode,
        string NewTagCategoryCode) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await tenantRepository.ExistsAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }

            if (await tagCategoryRepository.ExistsAsync(request.TenantCode, request.NewTagCategoryCode, cancellationToken))
            {
                throw new TagCategoryAlreadyExistsException(request.OldTagCategoryCode);
            }

            await tagCategoryRepository.RenameAsync(request.TenantCode, request.OldTagCategoryCode, request.NewTagCategoryCode, cancellationToken);
        }
    }
}