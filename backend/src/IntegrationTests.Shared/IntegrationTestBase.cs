using System.Reflection;
using AutoFixture;
using Core.Repositories;
using Infra;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Shared;

public abstract class BaseIntegrationTest
{
    protected static ITenantRepository TenantRepository = default!;
    protected static ITagCategoryRepository TagCategoryRepository = default!;
    protected static ITagRepository TagRepository = default!;
    protected static IProductRepository ProductRepository = default!;
    protected static IUserRepository UserRepository = default!;
    public static readonly FakeTimeProvider TimeProvider = new();

    protected BaseIntegrationTest()
    {
        ProductRepository = GetService<IProductRepository>();
        TagRepository = GetService<ITagRepository>();
        TenantRepository = GetService<ITenantRepository>();
        TagCategoryRepository = GetService<ITagCategoryRepository>();
        UserRepository = GetService<IUserRepository>();        
    }

    private static IServiceProvider _serviceProvider = null!;

    protected static Fixture Fixture { get; } = new();

    [AssemblyInitialize]
    public static void AssemblyInitialize(TestContext testContext)
    {
        var currentDirectory = Path.GetDirectoryName(typeof(BaseIntegrationTest).Assembly.Location)!;
    
        var appSettingsPath = Path.Combine(currentDirectory, "appsettings.json");

        var configuration = new ConfigurationBuilder()
            .AddJsonFile(appSettingsPath)
            .Build();
    
        _serviceProvider = new ServiceCollection()
            .AddInfra(configuration)
            .BuildServiceProvider();
    }
    
    [AssemblyCleanup]
    public static Task AssemblyCleanup()
    {
        return OperateInDocumentsFromMongoDbCollections(OperationType.DropCollection);
    }

    [TestCleanup]
    public async Task CleanUp()
    {
        await OperateInDocumentsFromMongoDbCollections(OperationType.DeleteMany);
    }

    protected static TService GetService<TService>()
        where TService : notnull
    {
        return _serviceProvider.GetRequiredService<TService>();
    }

    private static async Task OperateInDocumentsFromMongoDbCollections(OperationType operationType)
    {
        var database = _serviceProvider.GetRequiredService<IMongoDatabase>();

        using var cursor = await database.ListCollectionNamesAsync();
        while (await cursor.MoveNextAsync())
            foreach (var collectionName in cursor.Current)
            {
                var collection = database.GetCollection<BsonDocument>(collectionName);

                if (operationType == OperationType.DropCollection)
                    await database.DropCollectionAsync(collectionName);
                else
                    await collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
            }
    }

    private enum OperationType
    {
        DropCollection,
        DeleteMany
    }
}