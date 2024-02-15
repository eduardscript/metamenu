namespace Core.Entities;

public class Tag(int tenantCode, string tagCategoryCode, string code)
{
    public int TenantCode { get; set; } = tenantCode;

    public string TagCategoryCode { get; set; } = tagCategoryCode;

    public string Code { get; set; } = code;
}