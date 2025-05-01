using FluentValidation;
using MediatR;

namespace ires.Core.Behaviors
{
    public class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(async x => await x.ValidateAsync(context))
                .Select(x => x.Result)
                .SelectMany(x => x.Errors)
                .Where(x => x != null)
                .ToList();
            if (failures.Count != 0)
                throw new Exception(string.Join(". ", failures.Select(x => x.ErrorMessage)));

            return next();
        }
    }
}
