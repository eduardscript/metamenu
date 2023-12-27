namespace Core.Services;

public interface ITokenService
{
    public Task<string> GenerateTokenAsync(User user, CancellationToken cancellationToken);
}