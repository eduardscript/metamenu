namespace Core.Repositories;

public interface ITagRepository
{
    public class TagFilter
    {
        public TagFilter(int tenantCode, string? tagCategoryCode = null)
        {
            TenantCode = tenantCode;
            TagCategoryCode = tagCategoryCode;
        }

        public int TenantCode { get; set; }

        public string? TagCategoryCode { get; set; }
    }

    public Task CreateAsync(Tag tag, CancellationToken cancellationToken);

    public Task<IEnumerable<Tag>> GetAll(TagFilter tagFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, IEnumerable<string> tagCodes,
        CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(int tenantCode, string tagCode, CancellationToken cancellationToken);

    public Task RenameAsync(int tenantCode, string oldTagCode, string newTagCode, CancellationToken cancellationToken);
}