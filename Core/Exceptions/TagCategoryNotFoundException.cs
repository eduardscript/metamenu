namespace Core.Exceptions;

public class TagCategoryNotFoundException(string tagCategoryCode)
    : Exception($"Tag category with code {tagCategoryCode} was not found.");

public class TagCategoryAlreadyExistsException(string tagCategoryCode)
    : Exception($"Tag category with code {tagCategoryCode} already exists.");