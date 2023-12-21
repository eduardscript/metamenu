namespace Core.Repositories;

public interface ITagRepository
{
    public Task CreateAsync(Tag tag, CancellationToken cancellationToken);

    public Task<IEnumerable<Tag>> GetAllTags(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsByCodeAsync(int tenantCode, IEnumerable<string> tagCodes,
        CancellationToken cancellationToken);

    public Task<bool> ExistsByCodeAsync(int tenantCode, string tagCode, CancellationToken cancellationToken);
}