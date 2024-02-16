namespace Core.Repositories;

public class TagFilter(int tenantCode)
{
    public int TenantCode { get; set; } = tenantCode;

    public string? TagCategoryCode { get; set; }

    public string? Code { get; set; }

    public IEnumerable<string>? Codes { get; set; }
}

public enum UpdateType
{
    TagCategory,
    TagCodes
}

public class UpdateTagFilter(UpdateType updateType)
{
    public string? NewTagCategoryCode { get; set; }

    public UpdateType UpdateType { get; set; } = updateType;
}

public interface ITagRepository
{
    public Task<Tag> CreateAsync(Tag tag, CancellationToken cancellationToken);

    public Task<IEnumerable<Tag>> GetAllAsync(TagFilter tagFilter, CancellationToken cancellationToken);

    public Task<bool> ExistsAsync(
        TagFilter tagFilter,
        CancellationToken cancellationToken);

    public Task RenameAsync(int tenantCode, string oldTagCode, string newTagCode, CancellationToken cancellationToken);

    Task<bool> DeleteAsync(int requestTenantCode, string requestTagCategoryCode, string requestTagCode,
        CancellationToken cancellationToken);

    public Task<bool> UpdateManyAsync(
        TagFilter tagFilter,
        UpdateTagFilter updateFilter,
        CancellationToken cancellationToken);

    public Task<bool> DeleteManyAsync(TagFilter tagFilter, CancellationToken cancellationToken);
}