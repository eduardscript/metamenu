using MediatR.Pipeline;

namespace Core.Pipelines.PreProcessors;

public class ValidationRequestPreProcessor<TRequest>(IValidator<TRequest> validator)
    : IRequestPreProcessor<TRequest> where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(request, cancellationToken);
    }
}