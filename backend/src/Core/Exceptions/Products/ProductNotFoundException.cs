namespace Core.Exceptions.Products;

public class ProductNotFoundException(string productName)
    : Exception($"Product with name {productName} was not found.");