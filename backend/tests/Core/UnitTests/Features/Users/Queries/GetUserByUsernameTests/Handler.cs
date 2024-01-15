using Core.Exceptions.Products;
using Core.Features.Users.Queries;
using NSubstitute.ReturnsExtensions;

namespace UnitTests.Features.Users.Queries.GetUserByUsernameTests;

[TestClass]
public class Handler : TestBaseHandler<GetUserByUsername.Handler, GetUserByUsername.Query>
{
    [TestMethod]
    public async Task Handle_InvalidTenantCodes_ThrowsException()
    {
        // Arrange
        var user = Fixture.Create<User>();

        UserRepositoryMock.GetByAsync(user.Username, default)
            .ReturnsNull();

        // Act & Assert
        await AssertThrowsAsync<UserNotFoundException>(Request);
    }
}