namespace Core.Repositories;

public interface ITagRepository
{
    public Task CreateAsync(Tag tag, CancellationToken cancellationToken);

    public Task<IEnumerable<Tag>> GetAll(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, IEnumerable<string> tagCodes,
        CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, string tagCode, CancellationToken cancellationToken);

    public Task RenameAsync(int tenantCode, string oldTagCode, string newTagCode, CancellationToken cancellationToken);
}