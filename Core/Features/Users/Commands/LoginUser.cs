using Core.Exceptions.Products;
using Core.Services;

namespace Core.Features.Users.Commands;

public static class LoginUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmpty()
                .WithMessage("Username is required.");
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage("Password is required.");
        }
    }

    public record Command(
        string Username,
        string Password) : IRequest<UserTokenDto>;

    public class Handler(
        ITokenService tokenService,
        IUserRepository userRepository) : IRequestHandler<Command, UserTokenDto>
    {
        public async Task<UserTokenDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var existingUser = await userRepository.GetByAsync(request.Username, cancellationToken);
            if (existingUser is null)
            {
                throw new UserNotFoundException(request.Username);
            }
            
            if (!existingUser.Password.Equals(request.Password))
            {
                throw new InvalidPasswordException();
            }
            
            var token = await tokenService.GenerateTokenAsync(existingUser, cancellationToken);

            return new UserTokenDto(token);
        }
    }

    public record UserTokenDto(
        string Token);
}
