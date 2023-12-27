using Core.Services;
using Infra.Configuration;
using Infra.Helpers;
using Infra.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infra;

public static class DependencyInjection
{
    public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<JwtConfiguration>()
            .Bind(configuration.GetSection("Jwt"));
        
        services.AddOptions<MongoConfiguration>()
            .Bind(configuration.GetSection("MongoDb"));

        services.AddSingleton<ITokenService, TokenService>();

        return services
            .AddRepositories();
    }
}