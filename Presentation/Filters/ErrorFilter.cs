namespace Presentation.Filters;

public class ErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        return error
            .WithMessage(error.Message);
    }
}