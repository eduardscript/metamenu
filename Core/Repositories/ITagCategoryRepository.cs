namespace Core.Repositories;

public interface ITagCategoryRepository
{
    public Task CreateAsync(TagCategory tagCategory, CancellationToken cancellationToken);
    
    public Task<bool> ExistsByCodeAsync(string tagCategoryCode, CancellationToken cancellationToken);
}