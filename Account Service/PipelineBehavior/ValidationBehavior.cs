using FluentValidation;
using MediatR;

namespace Account_Service.PipelineBehavior
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validators"></param>
        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <inheritdoc />
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var context = new ValidationContext<TRequest>(request);
            var failures = _validators
                .Select(x => x.Validate(context))
                .SelectMany(x => x.Errors)
                .Where(x => x != null).ToList();

            if (failures.Any())
            {
                throw new ValidationException(failures);
            }

            return next(cancellationToken);
        }
    }
}
