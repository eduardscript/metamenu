using System.Security.Claims;

namespace Core.Authentication.UserAccessor;

/// <summary>
/// This interface is used to access the current user's claims and create a decoupling between the presentation layer and the core layer.
/// We use this interface to access the current user's claims in the core layer without having to reference the presentation layer. (avoiding framework dependencies)
/// </summary>
public interface IUserAccessor
{
    ClaimsPrincipal ClaimsPrincipal { get; set; }
}