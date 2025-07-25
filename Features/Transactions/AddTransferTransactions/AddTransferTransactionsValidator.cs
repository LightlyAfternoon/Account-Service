using FluentValidation;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    public class AddTransferTransactionsValidator : AbstractValidator<AddTransferTransactionsRequestCommand>
    {
        public AddTransferTransactionsValidator()
        {
            RuleFor(t => t.FromAccountId).NotEmpty();

            RuleFor(t => t.ToAccountId).NotEmpty();

            RuleFor(t => t.Sum).NotEmpty();

            RuleFor(t => t.Currency).NotEmpty();

            RuleFor(t => t.Description).NotEmpty();

            RuleFor(t => t.DateTime).NotEmpty();
        }
    }
}