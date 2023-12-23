namespace Core.Exceptions.Products;

public class ProductAlreadyExistsException(string productName)
    : Exception($"Product with name {productName} already exists.");