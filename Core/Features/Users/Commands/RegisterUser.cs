using Core.Exceptions.Tenants;
using Core.Features.Users.Shared;

namespace Core.Features.Users.Commands;

public static class RegisterUser
{
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