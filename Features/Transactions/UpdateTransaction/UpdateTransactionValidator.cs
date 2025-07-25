using FluentValidation;

namespace Account_Service.Features.Transactions.UpdateTransaction
{
    public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionRequestCommand>
    {
        public UpdateTransactionValidator()
        {
            RuleFor(t => t.Id).NotEmpty();

            RuleFor(t => t.Account).NotEmpty();

            RuleFor(t => t.CounterpartyAccount).NotEmpty();

            RuleFor(t => t.Sum).NotEmpty();

            RuleFor(t => t.Currency).NotEmpty();

            RuleFor(t => t.Type).NotEmpty();

            RuleFor(t => t.Description).NotEmpty();

            RuleFor(t => t.DateTime).NotEmpty();
        }
    }
}