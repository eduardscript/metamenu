namespace Core.Repositories;

public interface IUserRepository
{
    public Task<User?> GetByAsync(string username, CancellationToken cancellationToken);
    
    public Task<User> CreateAsync(User user, CancellationToken cancellationToken);
}