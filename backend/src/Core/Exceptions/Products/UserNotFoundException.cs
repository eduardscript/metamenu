namespace Core.Exceptions.Products;

public class UserNotFoundException(string username) : Exception($"User with username {username} was not found.");