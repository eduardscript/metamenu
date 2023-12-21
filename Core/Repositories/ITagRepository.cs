namespace Core.Repositories;

public interface ITagRepository
{
    public Task CreateAsync(Tag tag, CancellationToken cancellationToken);

    public Task<bool> ExistsByCodeAsync(IEnumerable<string> tagCodes, CancellationToken cancellationToken);
    
    public Task<bool> ExistsByCodeAsync(string tagCode, CancellationToken cancellationToken);
}