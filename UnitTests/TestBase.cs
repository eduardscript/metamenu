using Core.Services;
using MediatR;

namespace UnitTests;

public class TestBase
{
    protected static readonly Fixture Fixture = new();
}

public class TestBase<THandler> : TestBase where THandler : class
{
    protected readonly ITenantRepository TenantRepositoryMock = Substitute.For<ITenantRepository>();

    protected readonly ITagCategoryRepository TagCategoryRepositoryMock = Substitute.For<ITagCategoryRepository>();

    protected readonly ITagRepository TagRepositoryMock = Substitute.For<ITagRepository>();

    protected readonly IProductRepository ProductRepositoryMock = Substitute.For<IProductRepository>();

    protected readonly IUserRepository UserRepositoryMock = Substitute.For<IUserRepository>();

    protected readonly ITokenService TokenServiceMock = Substitute.For<ITokenService>();

    protected THandler Handler = null!;

    protected async Task AssertThrowsAsync<TException>(IBaseRequest command)
        where TException : Exception
    {
        var handlerType = Handler.GetType();

        var handlerExceptionMethod = handlerType.GetMethod("Handle")!;

        await Assert.ThrowsExceptionAsync<TException>(() => (Task)handlerExceptionMethod!.Invoke(Handler, [
            command, CancellationToken.None
        ])!);
    }
}

