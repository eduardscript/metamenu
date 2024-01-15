using FluentValidation;
using Presentation.Errors.Common;

namespace Presentation.Errors.Handlers;

public class ValidationExceptionErrorHandler : IErrorHandler<ValidationException>
{
    public IError HandleError(IError error, ValidationException exception)
    {
        return error
            .RemoveExtensions()
            .WithCode(ErrorFilterCodes.ModelValidationError)
            .WithMessage(exception.Message);
    }
}