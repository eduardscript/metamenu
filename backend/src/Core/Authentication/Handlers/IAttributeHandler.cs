namespace Core.Authentication.Handlers;

public interface IAttributeHandler<TAttribute>
    where TAttribute : Attribute
{
    Task HandleAsync<TRequest>(TRequest request, 
        IUserAccessor userAccessor,
        object propertyValue,
        CancellationToken cancellationToken)
        where TRequest : notnull;
}