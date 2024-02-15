using Core.Authentication.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Extensions.DependencyInjection;

public static class AddAttributeHandlersExtension
{
    private static readonly Type AttributeHandlerInterfaceType = typeof(IAttributeHandler<>);

    public static void AddAttributeHandlers(this IServiceCollection services)
    {
        var handlerTypes = GetAllAttributeHandlerTypes();

        RegisterAttributeHandlers(services, handlerTypes);
    }

    private static IEnumerable<Type> GetAllAttributeHandlerTypes()
    {
        return AssemblyReference.Assembly.GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.GetInterfaces().Any(i => i.IsGenericType &&
                                                         i.GetGenericTypeDefinition() ==
                                                         AttributeHandlerInterfaceType));
    }

    private static void RegisterAttributeHandlers(
        IServiceCollection services,
        IEnumerable<Type> handlerTypes)
    {
        foreach (var handlerType in handlerTypes)
        {
            try
            {
                var attributeType = GetAttributeType(handlerType, AttributeHandlerInterfaceType);
                var closedHandlerType = AttributeHandlerInterfaceType.MakeGenericType(attributeType);
                services.AddSingleton(closedHandlerType, handlerType);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Error registering attribute handler: {ex.Message}");
            }
        }
    }

    private static Type GetAttributeType(Type handlerType, Type attributeHandlerInterfaceType)
    {
        var interfaceType = handlerType.GetInterfaces()
            .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == attributeHandlerInterfaceType);

        return interfaceType.GetGenericArguments()[0];
    }
}