using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Presentation.Errors.Common;
using Presentation.Errors.Handlers;

namespace Presentation.Filters;

public class ErrorFilter : IErrorFilter
{
    [UnconditionalSuppressMessage(
        "ReflectionAnalysis", 
        "IL2026:RequiresUnreferencedCode",
        Justification = "This method uses reflection to dynamically create instances and invoke methods, which may not be compatible with trimming.")]
    public IError OnError(IError error)
    {
        var handlerType = GetExceptionHandlerType(error.Exception?.GetType()) ?? typeof(InternalServerErrorHandler);

        var handlerInstance = Activator.CreateInstance(handlerType);

        var handlerExceptionMethod = handlerType.GetMethod("HandleError")!;

        var result = (IError)handlerExceptionMethod.Invoke(handlerInstance, [error, error.Exception])!;
        
        return result;
    }

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicMethods)]
    [RequiresUnreferencedCode("Reflection is used to find types that implement IErrorHandler<T>. This can break if types are trimmed away.")]
    private static Type? GetExceptionHandlerType(Type? exceptionType)
    {
        if (exceptionType is null)
        {
            return null;
        }

        return Assembly.GetAssembly(typeof(Program))!
            .GetTypes()
            .Where(t => t.GetConstructor(Type.EmptyTypes) != null)
            .FirstOrDefault(IsExceptionHandlerType(exceptionType));
        
        static Func<Type, bool> IsExceptionHandlerType(Type exceptionType)
        {
            return type =>
                type.IsClass &&
                type.GetInterfaces().Any(IsGenericExceptionHandlerForType(exceptionType));
        }

        static Func<Type, bool> IsGenericExceptionHandlerForType(Type exceptionType)
        {
            return interfaceType =>
                interfaceType.IsGenericType &&
                interfaceType.GetGenericTypeDefinition() == typeof(IErrorHandler<>) &&
                interfaceType.GetGenericArguments()[0] == exceptionType;
        }
    }
}
