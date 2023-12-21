namespace Core.Repositories;

public interface ITagCategoryRepository
{
    public Task CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken);

    Task<IEnumerable<TagCategory>> GetAllTags(int tenantCode, CancellationToken cancellationToken);

    public Task<bool> ExistsByAsync(int tenantCode, string tagCode, CancellationToken cancellationToken);
}