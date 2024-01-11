namespace Core.Exceptions.TagCategories;

public class TagCategoryHasNoAssociatedEntitiesException(string tagCategoryCode, string[] tagCodes) : Exception(
    $"Tag category {tagCategoryCode} has no associated entities with tags {string.Join(", ", tagCodes)}");