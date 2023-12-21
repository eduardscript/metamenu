using Infra;
using Infra.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace IntegrationTests.Helpers;

[TestClass]
public class IntegrationTestBase : TestBase
{
    private static readonly IServiceProvider ServiceProvider;

    static IntegrationTestBase()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.development.json");

        var config = configuration.Build();

        ServiceProvider = new ServiceCollection()
            .AddInfra(config.GetRequiredSection("MongoDb").Get<MongoConfiguration>()!)
            .BuildServiceProvider();
    }

    [TestCleanup]
    public async Task CleanUp()
    {
        await OperateInDocumentsFromMongoDbCollections(OperationType.DeleteMany);
    }

    [AssemblyCleanup]
    public static Task DropAllMongoDbCollections()
    {
        return OperateInDocumentsFromMongoDbCollections(OperationType.DropCollection);
    }

    protected TService GetService<TService>()
        where TService : notnull
    {
        return ServiceProvider.GetRequiredService<TService>();
    }

    private enum OperationType
    {
        DropCollection,
        DeleteMany
    }

    private static async Task OperateInDocumentsFromMongoDbCollections(OperationType operationType)
    {
        var database = ServiceProvider.GetRequiredService<IMongoDatabase>();

        using var cursor = await database.ListCollectionNamesAsync();
        while (await cursor.MoveNextAsync())
        {
            foreach (var collectionName in cursor.Current)
            {
                var collection = database.GetCollection<BsonDocument>(collectionName);

                if (operationType == OperationType.DropCollection)
                {
                    await database.DropCollectionAsync(collectionName);
                }
                else
                {
                    await collection.DeleteManyAsync(Builders<BsonDocument>.Filter.Empty);
                }
            }
        }
    }
}