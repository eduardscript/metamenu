namespace Core.Authentication.Handlers;

public interface IPermissionValidationHandler
{
    Task HandleAsync<TRequest>(TRequest request,
        IUserAccessor userAccessor,
        object? propertyInfo,
        CancellationToken cancellationToken) where TRequest : notnull;
}