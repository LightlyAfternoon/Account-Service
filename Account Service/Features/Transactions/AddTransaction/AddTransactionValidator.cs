using Account_Service.Features.Accounts;
using FluentValidation;
using JetBrains.Annotations;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <inheritdoc />
    [UsedImplicitly]
    public class AddTransactionValidator : AbstractValidator<AddTransactionRequestCommand>
    {
        /// <inheritdoc />
        public AddTransactionValidator(IAccountsService accountService)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(t => t.AccountId).NotEmpty().WithMessage("Отсутствует id счёта, с которого происходит транзакция")
                .Must(t => accountService.FindById(t).Result != null).WithMessage("Счёт с данным id не существует");

            RuleFor(t => t).Must(t => t.Sum > 0).WithMessage("Отсутствует сумма транзакции или она меньше 0")
                .Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.AccountId).Result;

                    if (accountDto != null && Enum.Parse<TransactionType>(t.Type).Equals(TransactionType.Credit))
                        return t.Sum <= accountDto.Balance;

                    return true;
                }).WithMessage("Сумма транзакции больше текущего баланса на счёте");

            RuleFor(t => t.Currency).NotEmpty().WithMessage("Отсутствует валюта транзакции")
                .Must(type => Enum.TryParse(type, out CurrencyCode _))
                .WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(t => t.Type).NotEmpty().WithMessage("Отсутствует тип транзакции")
                .Must(type => Enum.TryParse(type, out TransactionType _))
                .WithMessage("Данный тип транзакции не существует");

            RuleFor(t => t.Description).NotEmpty().WithMessage("Отсутствует описание транзакции");

            RuleFor(t => t).Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.AccountId).Result;

                    if (accountDto != null)
                        return accountDto.CloseDate == null;

                    return true;
                }).WithMessage("Счёт закрыт")
                .Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.AccountId).Result;

                    if (accountDto != null)
                        return DateOnly.FromDateTime(t.DateTime) >= accountDto.OpenDate;

                    return true;
                }).WithMessage("Дата отправки транзакции не может быть раньше даты открытия счёта");
        }
    }
}