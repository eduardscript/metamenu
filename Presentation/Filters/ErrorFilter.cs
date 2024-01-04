using FluentValidation;
using Presentation.Constants;

namespace Presentation.Filters;

public class ErrorFilter : IErrorFilter
{
    private static readonly string EnvironmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")!;

    public IError OnError(IError error)
    {
        if (EnvironmentName == Environments.Development)
        {
            return HandleDevelopmentEnvironmentError(error);
        }

        if (error.Exception is ValidationException validationException)
        {
            return HandleValidationException(error, validationException);
        }

        return error.WithCode(ErrorFilterCodes.InternalServerError);
    }

    private static IError HandleDevelopmentEnvironmentError(IError error)
    {
        return error
            .WithCode(ErrorFilterCodes.InternalServerError)
            .WithExtensions(new Dictionary<string, object?>
            {
                { "StackTrace", error.Exception?.StackTrace }
            })
            .WithMessage(error.Exception?.Message ?? "An error occurred while processing your request.");
    }

    private static IError HandleValidationException(IError error, ValidationException validationException)
    {
        var errorsDictionary = new Dictionary<string, object>
        {
            { "Errors", validationException.Errors }
        };

        return error
            .WithCode(ErrorFilterCodes.ModelValidationError)
            .WithExtensions(errorsDictionary)
            .WithMessage("Validation failed for one or more entity properties. See 'Errors' for more details.");
    }
}