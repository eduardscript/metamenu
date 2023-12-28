using Core.Exceptions.Tenants;
using Core.Features.Users.Shared;
using Core.Validators;

namespace Core.Features.Users.Commands;

public static class RegisterUser
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

            RuleFor(x => x.AvailableTenants)
                .NotEmpty()
                .WithMessage("Available Tenants must not be empty.")
                .Unique()
                .WithMessage("AvailableTenants must be unique. Duplicated ones: {DuplicateItems}.");
        }
    }

    public record Command(
        string Username,
        string Password,
        IEnumerable<int> AvailableTenants) : IRequest<UserDto>;

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