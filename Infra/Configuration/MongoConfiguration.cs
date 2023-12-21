namespace Infra.Configuration;

public class MongoConfiguration
{
    public string ConnectionString { set; get; } = default!;

    public  string DatabaseName { set; get; } = default!;
}