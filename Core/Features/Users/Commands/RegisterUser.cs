using Core.Exceptions.Tenants;
using Core.Features.Users.Shared;

namespace Core.Features.Users.Commands;

public static class RegisterUser
{
    public class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Username)
                .NotEmptyAndRequired();

            RuleFor(c => c.Password)
                .NotEmptyAndRequired();

            RuleFor(c => c.AvailableTenants)
                .NotEmptyUniqueAndGreaterThanZero();
        }
    }

    public class Command(
        string username,
        string password,
        IEnumerable<int> availableTenants) : IRequest<UserDto>
    {
        public string Username { set; get; } = username;
        public string Password { set; get; } = password;
        public IEnumerable<int> AvailableTenants { set; get; } = availableTenants;
    }

    public class Handler(
        ITenantRepository tenantRepository,
        IUserRepository userRepository) : IRequestHandler<Command, UserDto>
    {
        public async Task<UserDto> Handle(Command request, CancellationToken cancellationToken)
        {
            var tenants = await tenantRepository.GetAllAsync(cancellationToken);

            var invalidTenantCodes = request.AvailableTenants
                .Except(tenants.Select(t => t.TenantCode))
                .ToList();

            if (invalidTenantCodes.Count > 0)
            {
                throw new TenantsNotFoundException(invalidTenantCodes);
            }

            var user = new User(
                request.Username,
                request.Password,
                request.AvailableTenants);

            var newUser = await userRepository.CreateAsync(user, cancellationToken);

            return newUser.ToDto();
        }
    }
}