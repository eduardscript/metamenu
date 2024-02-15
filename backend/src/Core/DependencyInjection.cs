using Core.Authentication;
using Core.Authentication.Helpers.Cache;
using Core.Authentication.UserAccessor;
using Core.Extensions.DependencyInjection;
using Core.PreProcessors;
using Microsoft.Extensions.DependencyInjection;

namespace Core;

public static class DependencyInjection
{
    public class CoreOptions
    {
        public bool UseAuth { get; set; }
    }

    public static IServiceCollection AddApplication(this IServiceCollection services, CoreOptions options)
    {
        services.AddValidatorsFromAssembly(AssemblyReference.Assembly);

        services
            .AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);

                if (options.UseAuth)
                {
                    cfg.AddOpenRequestPreProcessor(typeof(UserAccessorPreProcessor<>));
                }

                cfg.AddOpenRequestPreProcessor(typeof(ValidationRequestPreProcessor<>));
            });

        services.AddSingleton(TimeProvider.System);

        services.AddSingleton<IUserAccessor, UserAccessor>();

        services.AddAttributeHandlers();

        services.AddSingleton<IPropertyCache, PropertyCache>();

        return services;
    }
}