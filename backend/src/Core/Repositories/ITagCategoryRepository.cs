namespace Core.Repositories;

public interface ITagCategoryRepository
{
    public Task<TagCategory> CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken);

    Task<IEnumerable<TagCategory>> GetAllAsync(int tenantCode, CancellationToken cancellationToken);
    
    Task<TagCategory?> GetByAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken);
    
    public Task<bool> ExistsAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken);
    
    public Task RenameAsync(int tenantCode, string oldTagCategoryCode, string newCategoryCode, CancellationToken cancellationToken);
}