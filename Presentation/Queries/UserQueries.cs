using System.Security.Claims;
using Core.Features.Users.Queries;
using Core.Features.Users.Shared;
using HotChocolate.Authorization;

namespace Presentation.Queries;

[ExtendObjectType(RootTypes.Query)]
public class UserQueries
{
    [Authorize]
    public Task<UserDto> GetMe([Service] IMediator mediator, ClaimsPrincipal claimsPrincipal)
    {
        var username = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
        
        return mediator.Send(new GetUserByUsername.Query(username!));
    }
}