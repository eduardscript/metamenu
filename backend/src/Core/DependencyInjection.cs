using Core.Authentication;
using Core.Authentication.Handlers;
using Core.Authentication.Helpers.Cache;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
                cfg.AddOpenRequestPreProcessor(typeof(UserAccessorPreProcessor<>));
                cfg.AddOpenRequestPreProcessor(typeof(ValidationRequestPreProcessor<>));
            });

        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<IUserAccessor, UserAccessor>();
        services.AddSingleton<IPermissionValidationHandler, TenantPermissionValidationHandler>();
        services.AddSingleton<IPermissionValidationHandler, AdminPermissionValidationHandler>();
        services.AddSingleton<IPropertyCache, PropertyCache>();

        return services;
    }
}

