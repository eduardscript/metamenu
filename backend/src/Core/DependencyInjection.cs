using Core.Authentication;
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

        return services;
    }
}

