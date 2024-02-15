using Presentation.Errors.Common;

namespace Presentation.Errors.Handlers;

public class InternalServerErrorHandler : IErrorHandler<Exception>
{
    public IError HandleError(IError error, Exception exception)
    {
        if (error.Code == "AUTH_NOT_AUTHORIZED")
        {
            return error
                .WithCode(error.Code)
                .WithMessage(error.Message);
        }
        
        return error
            .WithCode(ErrorFilterCodes.InternalServerError)
            .WithMessage("Internal Server Error");
    }
}