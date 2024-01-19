namespace Core.Repositories;

public class TagFilter(int tenantCode, string? tagCategoryCode = null)
{
    public int TenantCode { get; set; } = tenantCode;

    public string? TagCategoryCode { get; set; } = tagCategoryCode;
}

public interface ITagRepository
{
    public Task<Tag> CreateAsync(Tag tag, CancellationToken cancellationToken);

    public Task<IEnumerable<Tag>> GetAllAsync(TagFilter tagFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(
        int tenantCode, 
        IEnumerable<string> tagCodes,
        CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, string tagCategoryCode, CancellationToken cancellationToken);

    public Task RenameAsync(int tenantCode, string oldTagCode, string newTagCode, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int requestTenantCode, string requestTagCategoryCode, string requestTagCode, CancellationToken cancellationToken);
}