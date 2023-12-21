using System.Reflection;
using Core;
using Core.Entities;
using Humanizer;
using Infra.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Tag = Core.Entities.Tag;

namespace Infra.Helpers;

public static class DependencyInjectionHelpers
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetRequiredService<MongoConfiguration>();

        return services
            .AddMongoDb(configuration)
            .AddCollection<Tenant>()
            .AddCollection<TagCategory>()
            .AddCollection<Tag>()
            .AddCollection<Product>();
    }

    private static IServiceCollection AddMongoDb(this IServiceCollection services, MongoConfiguration configuration)
    {
        var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
        ConventionRegistry.Register(
            "IgnoreExtraElements",
            conventionPack,
            _ => true);

        services.AddScoped<IMongoClient>(_ => new MongoClient(configuration.ConnectionString));
        services.AddScoped(provider =>
            provider.GetRequiredService<IMongoClient>().GetDatabase(configuration.DatabaseName));

        return services;
    }

    private static IServiceCollection AddCollection<TEntity>(
        this IServiceCollection services,
        string? collectionName = null)
        where TEntity : class
    {
        collectionName ??= typeof(TEntity).Name.Pluralize();

        var repositoriesAbstractions = AssemblyReference.Assembly
            .GetTypes()
            .Where(x => x.IsInterface && x.Name.EndsWith("Repository"))
            .OrderBy(x => x.Name)
            .ToArray();

        var repositoriesImplementations = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(x => x.IsClass && x.Name.EndsWith("Repository"))
            .OrderBy(x => x.Name)
            .ToArray();

        for (var i = 0; i < repositoriesAbstractions.Length; i++)
            services.AddScoped(repositoriesAbstractions[i], repositoriesImplementations[i]);

        return services
            .AddSingleton<IMongoCollection<TEntity>>(
                sp =>
                {
                    var mongoDb = services.BuildServiceProvider().GetRequiredService<IMongoDatabase>();

                    return mongoDb.GetCollection<TEntity>(collectionName);
                });
    }
}