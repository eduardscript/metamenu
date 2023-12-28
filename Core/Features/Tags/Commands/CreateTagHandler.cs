using Core.Exceptions.TagCategories;
using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;

namespace Core.Features.Tags.Commands;

public static class CreateTagHandler
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

            if (!await tenantRepository.ExistsByAsync(tag.TenantCode, cancellationToken))
                throw new TenantNotFoundException(tag.TenantCode);

            if (!await tagCategoryRepository.ExistsByAsync(tag.TenantCode, tag.TagCategoryCode, cancellationToken))
                throw new TagCategoryNotFoundException(tag.TagCategoryCode);

            await tagRepository.CreateAsync(tag, cancellationToken);
        }
    }
}