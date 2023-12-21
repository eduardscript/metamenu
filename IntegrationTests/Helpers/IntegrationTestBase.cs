using Infra;
using Infra.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntegrationTests.Helpers;

public abstract class IntegrationTestBase : TestBase, IAsyncLifetime
{
    private readonly IServiceProvider _serviceProvider;

    protected IntegrationTestBase()
    {
        var configuration =  new ConfigurationBuilder()
            .AddJsonFile("appsettings.development.json");
        
        var config = configuration.Build();
        
        _serviceProvider = new ServiceCollection()
            .AddInfra(config.GetRequiredSection("MongoDb").Get<MongoConfiguration>()!)
            .AddOptions()
            .BuildServiceProvider();
    }

    protected TService GetService<TService>()
        where TService : notnull
    {
        return _serviceProvider.GetRequiredService<TService>();
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => DeleteAllDocumentsFromMongoDbCollections();

    private async Task DeleteAllDocumentsFromMongoDbCollections()
    {
        var database = _serviceProvider.GetRequiredService<IMongoDatabase>();

        using var cursor = await database.ListCollectionNamesAsync();
        while (await cursor.MoveNextAsync())
        {
            foreach (var collectionName in cursor.Current)
            {
                var collection = database.GetCollection<BsonDocument>(collectionName);

                // Include a brief pause between delete operations to mitigate potential issues
                // related to eventual consistency in distributed database environments.
                // This delay helps ensure that each delete operation has sufficient time to be 
                // recognized across the cluster, thereby preventing race conditions or read inconsistencies
                // that might occur in rapid succession of delete commands, especially in testing scenarios.
                await Task.Delay(500);
                await collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
            }
        }
    }
    
    private async Task DropAllMongoDbCollections()
    {
        var database = _serviceProvider.GetRequiredService<IMongoDatabase>();
        
        using var cursor = await database.ListCollectionNamesAsync();
        while (await cursor.MoveNextAsync())
        {
            foreach (var collectionName in cursor.Current)
            {
                await database.DropCollectionAsync(collectionName);
            }
        }
    }
}