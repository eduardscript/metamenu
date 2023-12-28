using Core.Entities;
using Core.Repositories;
using MongoDB.Driver;

namespace Infra.Repositories;

public class UserRepository(IMongoCollection<User> collection) : IUserRepository
{
    public async Task<User?> GetByAsync(string username, CancellationToken cancellationToken)
    {
        var user = await collection
            .Find(u => u.Username == username)
            .FirstOrDefaultAsync(cancellationToken);

        return user;
    }

    public Task<User> CreateAsync(User user, CancellationToken cancellationToken)
    {
        return collection
            .InsertOneAsync(user, cancellationToken: cancellationToken)
            .ContinueWith(_ => user, cancellationToken);
    }
}