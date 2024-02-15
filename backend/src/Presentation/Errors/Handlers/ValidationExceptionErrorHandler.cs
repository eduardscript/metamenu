using System.Text.Json;
using FluentValidation;
using Presentation.Errors.Common;

namespace Presentation.Errors.Handlers;

public class ValidationExceptionErrorHandler : IErrorHandler<ValidationException>
{
    public IError HandleError(IError error, ValidationException exception)
    {
        var errors = exception.Errors.GroupBy(e => e.PropertyName)
            .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

        return error
            .RemoveExtensions()
            .WithCode(ErrorFilterCodes.ModelValidationError)
            .SetExtension("errors", JsonSerializer.Serialize(errors))
            .WithMessage(exception.Message);
    }
}