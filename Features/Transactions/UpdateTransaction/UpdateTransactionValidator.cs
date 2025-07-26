using Account_Service.Features.Accounts;
using FluentValidation;

namespace Account_Service.Features.Transactions.UpdateTransaction
{
    /// <inheritdoc />
    public class UpdateTransactionValidator : AbstractValidator<UpdateTransactionRequestCommand>
    {
        /// <inheritdoc />
        public UpdateTransactionValidator(IAccountService accountService)
        {
            RuleFor(t => t.Id).NotEmpty().WithMessage("Отсутствует id транзакции");

            RuleFor(t => t.CounterpartyAccountId).NotEmpty().WithMessage("Отсутствует id счёта, на который отправляются деньги");

            RuleFor(t => t.Sum).NotEmpty().WithMessage("Отсутствует сумма транзакции");

            RuleFor(t => t.Currency).NotEmpty().WithMessage("Отсутствует валюта транзакции");

            RuleFor(a => a.Currency).Must(type => Enum.TryParse(type, out CurrencyCode _)).WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(t => t.Type).NotEmpty().WithMessage("Отсутствует тип транзакции");

            RuleFor(a => a.Type).Must(type => Enum.TryParse(type, out TransactionType _)).WithMessage("Данный тип транзакции не существует");

            RuleFor(t => t.Description).NotEmpty().WithMessage("Отсутствует описание транзакции");

            RuleFor(t => t.DateTime).NotEmpty().WithMessage("Отсутствует дата и время отправки транзакции");

            RuleFor(a => accountService.FindById(a.AccountId)).NotEmpty().WithMessage("Счёт с данным id не существует");

            RuleFor(t => accountService.FindById(t.CounterpartyAccountId)).NotEmpty()
                .When(t => t.CounterpartyAccountId != Guid.Empty).WithMessage("Счёт с данным id не существует");

            RuleFor(a => a.AccountId).NotEqual(a => a.CounterpartyAccountId).WithMessage("Счёт, с которого, и счёт, на который отправляются деньги, не могут быть одним и тем же счётом");
        }
    }
}