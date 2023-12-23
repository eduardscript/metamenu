namespace Core.Repositories;

public interface ITagCategoryRepository
{
    public Task RenameAsync(int tenantCode, string oldTagCategoryCode, string newCategoryCode, CancellationToken cancellationToken);

    public Task CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken);

    Task<IEnumerable<TagCategory>> GetAll(int tenantCode, CancellationToken cancellationToken);
    
    public Task<bool> ExistsByAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken);
}