using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;

namespace Core.Features.TagCategories.Commands;

public static class CreateTagCategoryHandler
{
    public record Command(
        int TenantCode,
        string TagCategoryCode) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var tagCategory = new TagCategory(request.TenantCode, request.TagCategoryCode);

            if (!await tenantRepository.ExistsByAsync(tagCategory.TenantCode, cancellationToken))
                throw new TenantNotFoundException(tagCategory.TenantCode);

            if (await tagCategoryRepository.ExistsByAsync(tagCategory.TenantCode, tagCategory.TagCategoryCode,
                    cancellationToken)) throw new TagCategoryAlreadyExistsException(tagCategory.TagCategoryCode);

            await tagCategoryRepository.CreateAsync(tagCategory, cancellationToken);
        }
    }
}