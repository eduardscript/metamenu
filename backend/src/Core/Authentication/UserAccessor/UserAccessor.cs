using System.Security.Claims;

namespace Core.Authentication;

public class UserAccessor : IUserAccessor
{
    public ClaimsPrincipal ClaimsPrincipal { get; set; } = null!;
}