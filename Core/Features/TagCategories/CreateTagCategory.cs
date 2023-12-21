namespace Core.Features.TagCategories;

public static class CreateTagCategory
{
    public record Command(
        int TenantCode,
        string Code) : IRequest;

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
            
            if (await tagCategoryRepository.ExistsByCodeAsync(request.Code, cancellationToken))
            {
                throw new TagCategoryAlreadyExistsException(request.Code);
            }

            var tagCategory = new TagCategory(request.TenantCode, request.Code);

            await tagCategoryRepository.CreateAsync(tagCategory, cancellationToken);
        }
    }
}

// 1. Criar tenant
// 2, Criar tag category
// 3. Criar tag
// 4. Criar products (associar tags)