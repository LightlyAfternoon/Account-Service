using Account_Service.Features.Accounts;
using FluentValidation;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <inheritdoc />
    public class AddTransactionValidator : AbstractValidator<AddTransactionRequestCommand>
    {
        /// <inheritdoc />
        public AddTransactionValidator(IAccountService accountService)
        {
            RuleFor(t => t.AccountId).NotEmpty().WithMessage("Отсутствует id счёта, с которого происходит транзакция");

            RuleFor(t => t.Sum).NotEmpty().WithMessage("Отсутствует сумма транзакции");

            RuleFor(t => t.Currency).NotEmpty().WithMessage("Отсутствует валюта транзакции");

            RuleFor(a => a.Currency).Must(type => Enum.TryParse(type, out CurrencyCode _)).WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(t => t.Type).NotEmpty().WithMessage("Отсутствует тип транзакции");

            RuleFor(a => a.Type).Must(type => Enum.TryParse(type, out TransactionType _)).WithMessage("Данный тип транзакции не существует");

            RuleFor(t => t.Description).NotEmpty().WithMessage("Отсутствует описание транзакции");

            RuleFor(t => t.DateTime).NotEmpty().WithMessage("Отсутствует дата и время отправки транзакции");

            RuleFor(t => accountService.FindById(t.AccountId).Result).NotEmpty().WithMessage("Счёт с данным id не существует");
        }
    }
}