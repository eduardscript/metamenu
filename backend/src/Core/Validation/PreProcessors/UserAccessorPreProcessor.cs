using Core.Authentication;
using Core.Authentication.Handlers;
using Core.Authentication.Helpers.Cache;
using MediatR.Pipeline;

namespace Core.Validation.PreProcessors;

public class UserAccessorPreProcessor<TRequest>(
    IUserAccessor userAccessor,
    IEnumerable<IPermissionValidationHandler> handlers,
    IPropertyCache propertyCache)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var cachedProperties = propertyCache.GetCachedProperties(typeof(TRequest)).ToList();

        foreach (var handler in handlers)
        {
            foreach (var propertyInfo in cachedProperties)
            {
                foreach (var propertyInfoAttribute in propertyInfo.Attributes)
                {
                    await handler.HandleAsync(request, userAccessor,  propertyInfo.PropertyInfo.GetValue(request), cancellationToken);
                }
            }
        }
    }
}