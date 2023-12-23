using Core.Exceptions.TagCategories;
using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;

namespace Core.Features.Tags.Commands;

public static class RenameTagCode
{
    public record Command(
        int TenantCode,
        string OldTagCode,
        string NewTagCode) : IRequest;

    public class Handler(
        ITenantRepository tenantRepository,
        ITagRepository tagRepository) : IRequestHandler<Command>
    {
        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            if (!await tenantRepository.ExistsByAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }
            
            if (await tagRepository.ExistsByAsync(request.TenantCode, request.NewTagCode, cancellationToken))
            {
                throw new TagAlreadyExistsException(request.NewTagCode);
            }

            await tagRepository.RenameAsync(request.TenantCode, request.OldTagCode, request.NewTagCode, cancellationToken);
        }
    }
}