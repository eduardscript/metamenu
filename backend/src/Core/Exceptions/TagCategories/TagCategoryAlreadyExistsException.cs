namespace Core.Exceptions.TagCategories;

public class TagCategoryAlreadyExistsException(string tagCategoryCode)
    : Exception($"Tag category with code {tagCategoryCode} already exists.");