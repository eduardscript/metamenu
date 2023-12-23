namespace Core.Exceptions.Tags;

public class TagNotFoundException(IEnumerable<string> tagCodes)
    : Exception($"Tag codes {string.Join(",", tagCodes)} not found.");