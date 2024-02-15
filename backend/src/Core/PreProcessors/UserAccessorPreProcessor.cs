using System.Reflection;
using Core.Authentication.Handlers;
using Core.Authentication.Helpers.Cache;
using Core.Authentication.UserAccessor;
using Core.Exceptions.Extensions;
using MediatR.Pipeline;

namespace Core.PreProcessors;

public class UserAccessorPreProcessor<TRequest>(
    IUserAccessor userAccessor,
    IPropertyCache propertyCache,
    IServiceProvider serviceProvider)
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        var cachedProperties = propertyCache.GetCachedProperties(typeof(TRequest)).ToList();

        foreach (var propertyInfo in cachedProperties)
        {
            foreach (var attribute in propertyInfo.Attributes)
            {
                var handler =
                    serviceProvider.GetService(typeof(IAttributeHandler<>).MakeGenericType(attribute.GetType()));

                if (handler is null)
                {
                    throw new NoHandlerFoundException(attribute.GetType().Name);
                }

                try
                {
                    var handleAsyncMethod = handler.GetType().GetMethod("HandleAsync")?
                        .MakeGenericMethod(typeof(TRequest));

                    await (Task)handleAsyncMethod!.Invoke(handler, [
                        request,
                        userAccessor,
                        propertyInfo.PropertyInfo.GetValue(request),
                        cancellationToken
                    ])!;
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException!;
                }
            }
        }
    }
}