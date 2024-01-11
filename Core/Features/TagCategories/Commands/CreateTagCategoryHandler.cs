using Core.Exceptions.TagCategories;
using Core.Exceptions.Tenants;
using Core.Features.TagCategories.Queries;
using Core.Features.TagCategories.Shared;

namespace Core.Features.TagCategories.Commands;

public static class CreateTagCategoryHandler
{
    public record Command(
        int TenantCode,
        string Code) : IRequest<TagCategoryDto>;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagCategoryRepository tagCategoryRepository) : IRequestHandler<Command, TagCategoryDto>
    {
        public async Task<TagCategoryDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tagCategory = new TagCategory(request.TenantCode, request.Code);

            if (!await tenantRepository.ExistsAsync(tagCategory.TenantCode, cancellationToken))
                throw new TenantNotFoundException(tagCategory.TenantCode);

            if (await tagCategoryRepository.ExistsAsync(tagCategory.TenantCode, tagCategory.Code,
                    cancellationToken)) throw new TagCategoryAlreadyExistsException(tagCategory.Code);

            await tagCategoryRepository.CreateAsync(tagCategory, cancellationToken);

            return tagCategory.ToDto();
        }
    }
}