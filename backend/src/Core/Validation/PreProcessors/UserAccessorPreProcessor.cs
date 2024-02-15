using System.Reflection;
using Core.Authentication;
using Core.Authentication.Handlers;
using Core.Authentication.Helpers.Cache;
using MediatR.Pipeline;

namespace Core.Validation.PreProcessors;

public class UserAccessorPreProcessor<TRequest>(IUserAccessor userAccessor, IPropertyCache propertyCache)
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
                var attributeType = attribute.GetType();

                var handlerType = FindHandlerTypeForAttribute(attributeType);

                if (handlerType != null)
                {
                    var handler = Activator.CreateInstance(handlerType);

                    if (handler != null)
                    {
                        try
                        {
                            var handleAsyncMethod = handler
                                .GetType()
                                .GetMethod("HandleAsync")
                                ?.MakeGenericMethod(typeof(TRequest));

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
    }

    private Type? FindHandlerTypeForAttribute(Type attributeType)
    {
        var handlerInterfaceType = typeof(IAttributeHandler<>).MakeGenericType(attributeType);

        var handlerTypes = AssemblyReference.Assembly.GetTypes()
            .Where(type => handlerInterfaceType.IsAssignableFrom(type) && type is { IsInterface: false, IsAbstract: false });

        return handlerTypes.FirstOrDefault();
    }
}