using FluentValidation;

namespace Account_Service.Features.Transactions.AddTransaction
{
    public class AddTransactionValidator : AbstractValidator<AddTransactionRequestCommand>
    {
        public AddTransactionValidator()
        {
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