namespace Core.Exceptions.Tags;

public class TagAlreadyExistsException(string tagCode)
    : Exception($"Tag with code {tagCode} already exists.");