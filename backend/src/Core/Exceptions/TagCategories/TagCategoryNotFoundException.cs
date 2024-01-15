namespace Core.Exceptions.TagCategories;

public class TagCategoryNotFoundException(string tagCategoryCode)
    : Exception($"Tag category with code {tagCategoryCode} was not found.");