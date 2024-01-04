using MediatR.Pipeline;

namespace Core.Validation.PreProcessors;

public class ValidationRequestPreProcessor<TRequest>(IValidator<TRequest>? validator = null)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        if (validator != null)
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
        }
    }
}