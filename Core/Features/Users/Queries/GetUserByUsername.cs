using Core.Features.Users.Shared;

namespace Core.Features.Users.Queries;

public static class GetUserByUsername
{
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.");
        }
    }
    
    public record Query(
        string Username) : IRequest<UserDto>;

    public class Handler(IUserRepository userRepository) : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByAsync(request.Username, cancellationToken);

            return user.ToDto();
        }
    }
}