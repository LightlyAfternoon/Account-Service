using Account_Service.Features.Accounts;
using FluentValidation;
using JetBrains.Annotations;

namespace Account_Service.Features.Transactions.AddTransaction
// ReSharper disable once ArrangeNamespaceBody
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
                    var accountDto = accountService.FindById(t.AccountId).Result;

                    if (accountDto != null && t.Type.Equals(nameof(TransactionType.Debit)))
                        return t.Sum <= accountDto.Balance;

                    return true;
                }).WithMessage("Сумма транзакции больше текущего баланса на счёте");

            RuleFor(t => t.Currency).NotEmpty().WithMessage("Отсутствует валюта транзакции")
                .Must(type => Enum.TryParse(type, out CurrencyCode _))
                .WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(t => t)
                .Must(t => !string.IsNullOrWhiteSpace(t.Type)).WithMessage("Отсутствует тип транзакции")
                .Must(t => Enum.TryParse(t.Type, out TransactionType _))
                .WithMessage("Данный тип транзакции не существует")
                .Must(t =>
                {
                    var accountDto = accountService.FindById(t.AccountId).Result;

                    if (accountDto != null && t.Type.Equals(nameof(TransactionType.Debit)))
                        return !accountDto.Frozen;

                    return true;
                }).WithMessage("С замороженного счёта нельзя снимать деньги");

            RuleFor(t => t.Description).NotEmpty().WithMessage("Отсутствует описание транзакции");

            RuleFor(t => t).Must(t =>
                {
                    var accountDto = accountService.FindById(t.AccountId).Result;

                    return accountDto?.CloseDate == null;
                }).WithMessage("Счёт закрыт")
                .Must(t =>
                {
                    var accountDto = accountService.FindById(t.AccountId).Result;

                    if (accountDto != null)
                        return DateOnly.FromDateTime(t.DateTime) >= accountDto.OpenDate;

                    return true;
                }).WithMessage("Дата отправки транзакции не может быть раньше даты открытия счёта");
        }
    }
}