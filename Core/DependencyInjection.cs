using Core.Pipelines.PreProcessors;
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
                cfg.AddOpenRequestPreProcessor(typeof(ValidationRequestPreProcessor<>));
            });

        return services;
    }
}

