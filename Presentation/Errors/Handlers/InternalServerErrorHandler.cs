using Presentation.Errors.Common;

namespace Presentation.Errors.Handlers;

public class InternalServerErrorHandler : IErrorHandler<Exception>
{
    public IError HandleError(IError error, Exception exception)
    {
        return error
            .WithCode(ErrorFilterCodes.InternalServerError)
            .WithMessage("Internal Server Error");
    }
}