using Core.Authentication.UserAccessor;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;

namespace Presentation.Interceptors;

public class HttpContextUserAccessorInterceptor(IUserAccessor userAccessor) : DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        userAccessor.ClaimsPrincipal = context.User;

        return base.OnCreateAsync(context, requestExecutor, requestBuilder,
            cancellationToken);
    }
}

