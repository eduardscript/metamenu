using Core.Exceptions.Tags;
using Core.Exceptions.Tenants;

namespace Core.Features.Tags.Commands;

public static class RenameTagCodeHandler
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
            if (!await tenantRepository.ExistsAsync(request.TenantCode, cancellationToken))
            {
                throw new TenantNotFoundException(request.TenantCode);
            }
            
            if (await tagRepository.ExistsAsync(request.TenantCode, request.NewTagCode, cancellationToken))
            {
                throw new TagAlreadyExistsException(request.NewTagCode);
            }

            await tagRepository.RenameAsync(request.TenantCode, request.OldTagCode, request.NewTagCode, cancellationToken);
        }
    }
}