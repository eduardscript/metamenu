using Core.Services;
using MediatR;

namespace UnitTests.Helpers;

public class TestBaseHandler<THandler, TRequest> :
    TestBase where THandler : class
    where TRequest : class, IBaseRequest
{
    protected readonly ITenantRepository TenantRepositoryMock = Substitute.For<ITenantRepository>();

    protected readonly ITagCategoryRepository TagCategoryRepositoryMock = Substitute.For<ITagCategoryRepository>();

    protected readonly ITagRepository TagRepositoryMock = Substitute.For<ITagRepository>();

    protected readonly IProductRepository ProductRepositoryMock = Substitute.For<IProductRepository>();

    protected readonly IUserRepository UserRepositoryMock = Substitute.For<IUserRepository>();

    protected readonly ITokenService TokenServiceMock = Substitute.For<ITokenService>();

    protected THandler Handler = default!;

    protected static readonly TRequest Request = Fixture.Create<TRequest>();

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