using Core.Exceptions.Products;
using Core.Features.Users.Shared;

namespace Core.Features.Users.Queries;

public static class GetUserByUsername
{
    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmptyAndRequired();
        }
    }

    public class Query(
        string username) : IRequest<UserDto>
    {
        public string Username { get; set; } = username;
    }

    public class Handler(IUserRepository userRepository) : IRequestHandler<Query, UserDto>
    {
        public async Task<UserDto> Handle(Query request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByAsync(request.Username, cancellationToken);
            if (user is null)
            {
                throw new UserNotFoundException(request.Username);
            }

            return user.ToDto();
        }
    }
}