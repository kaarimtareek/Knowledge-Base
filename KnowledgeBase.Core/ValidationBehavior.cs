using Common;
using FluentValidation;
using MediatR;

namespace KnowledgeBase.Core;


public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : OperationResult, new() // Ensure our response type is an OperationResult
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        System.Diagnostics.Debug.WriteLine("ValidationBehavior");
        _validators = validators;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!_validators.Any())
        {
            // If there are no validators for this request, just continue to the handler
            return await next();
        }

        var context = new ValidationContext<TRequest>(request);

        var validationFailures = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var errors = validationFailures
            .SelectMany(r => r.Errors)
            .Where(f => f != null)
            .ToList();

        if (errors.Count != 0)
        {
            // If there are validation errors, short-circuit the pipeline
            // and return a failure result before the handler is ever executed.
            var errorMessages = string.Join(" | ", errors.Select(e => e.ErrorMessage));
            
            // We need a way to create a generic OperationResult.Failure<T>.
            // This requires a small adjustment to OperationResult or a factory.
            // For now, let's assume a way to create a generic failure.
            // A simple way is to use reflection, or add a constructor to TResponse.
            var resultType = typeof(TResponse);
            var failureMethod = resultType.GetMethod("Failure", new[] { typeof(string) });
            if (failureMethod != null)
            {
                return (TResponse)failureMethod.Invoke(null, new object[] { errorMessages });
            }
            
            // Fallback if the generic Failure method isn't found
            var failureResult = new TResponse(); // Assumes TResponse has a parameterless constructor
            typeof(OperationResult).GetProperty("Message")?.SetValue(failureResult, errorMessages);
            return failureResult;
        }

        // If validation succeeds, call the next delegate in the pipeline (which could be another behavior or the actual handler)
        return await next();
    }
}