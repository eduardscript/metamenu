using Infra.Configuration;
using Infra.Helpers;
using Microsoft.Extensions.DependencyInjection;
namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, MongoConfiguration configure)
    {
        services.AddSingleton(configure);

        return services
            .AddRepositories();
    }
}