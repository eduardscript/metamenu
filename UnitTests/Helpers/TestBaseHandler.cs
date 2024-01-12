using MediatR;

namespace UnitTests.Helpers;

public class TestBaseHandler<THandler, TRequest> :
    TestBase where THandler : class
    where TRequest : class, IBaseRequest
{
    protected THandler Handler = default!;

    protected static readonly TRequest Request = Fixture.Create<TRequest>();

    [TestInitialize]
    public void TestInitialize()
    {
        Handler = Build<THandler>();
    }

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