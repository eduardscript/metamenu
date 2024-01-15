namespace Core.Features.Users.Shared;

public record UserDto(
    Guid UserId,
    string Username,
    IEnumerable<int> AvailableTenants,
    DateTime CreatedAt);
    
public static class UserDtoExtensions
{
    public static UserDto ToDto(this User user)
    {
        return new UserDto(
            user.UserId,
            user.Username,
            user.AvailableTenants,
            user.CreatedAt);
    }
}