﻿using Core.Exceptions.Products;
using Core.Services;

namespace Core.Features.Users.Commands;

public static class LoginUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(x => x.Username)
                .NotEmptyAndRequired();

            RuleFor(x => x.Password)
                .NotEmptyAndRequired();
        }
    }

    public class Command(
        string username,
        string password) : IRequest<UserTokenDto>
    {
        public string Username { get; set; } = username;
        
        public string Password { get; set; } = password;
    }

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
