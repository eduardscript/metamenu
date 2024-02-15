namespace Core.Exceptions.Extensions;

public class NoHandlerFoundException(string handler) : Exception($"No handler found for attribute: {handler}");