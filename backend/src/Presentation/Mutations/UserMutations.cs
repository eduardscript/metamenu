using Core.Features.Users.Commands;
using Core.Features.Users.Shared;

namespace Presentation.Mutations;

[ExtendObjectType(RootTypes.Mutation)]
public class UserMutations
{
    public Task<LoginUser.UserTokenDto> Login([Service] IMediator mediator, LoginUser.Command command)
    {
        return mediator.Send(command);
    }
    
    public Task<UserDto> Register([Service] IMediator mediator, RegisterUser.Command command)
    {
        return mediator.Send(command);
    }
}