using System.Diagnostics.CodeAnalysis;
using HotChocolate.Types.Descriptors;

namespace Presentation.Conventions;

public class CustomNamingConvention : DefaultNamingConventions
{
    public override string GetTypeName(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
        Type type, TypeKind kind)
    {
        if (kind == TypeKind.InputObject && ImplementsIRequest(type))
        {
            return type.FullName![(type.FullName.LastIndexOf('.') + 1)..].Replace("+", "");
            ;
        }

        return base.GetTypeName(type, kind);
    }

    private static bool ImplementsIRequest(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.Interfaces)]
        Type type)
    {
        // Check if the type directly implements IRequest
        if (typeof(IRequest).IsAssignableFrom(type)) return true;

        // Check if the type implements any generic IRequest<>
        var interfaces = type.GetInterfaces();
        return interfaces.Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRequest<>));
    }
}