using Account_Service.Features.Accounts;
using FluentValidation;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    /// <inheritdoc />
    // ReSharper disable once GrammarMistakeInComment
    // ReSharper disable once UnusedMember.Global Используется FluentValidator во время выполнения программы
    public class AddTransferTransactionsValidator : AbstractValidator<AddTransferTransactionsRequestCommand>
    {
        /// <inheritdoc />
        public AddTransferTransactionsValidator(IAccountService accountService)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(t => t.FromAccountId).NotEmpty()
                .WithMessage("Отсутствует id счёта, с которого происходит списание денег")
                .Must(t => accountService.FindById(t).Result != null).WithMessage("Счёт с данным id не существует")
                .NotEqual(t => t.ToAccountId)
                .WithMessage(
                    "Счёт, с которого, и счёт, на который отправляются деньги, не могут быть одним и тем же счётом");

            RuleFor(t => t.ToAccountId).NotEmpty()
                .WithMessage("Отсутствует id счёта, на который происходит зачисление денег")
                .Must(t => accountService.FindById(t).Result != null).WithMessage("Счёт с данным id не существует");

            RuleFor(t => t).Must(t => t.Sum > 0).WithMessage("Отсутствует сумма транзакции")
                .Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.FromAccountId).Result;

                    if (accountDto != null)
                        return t.Sum <= accountDto.Balance;

                    return true;
                }).WithMessage("Сумма транзакции больше текущего баланса на счёте, с которого происходит списание");

            RuleFor(t => t.Currency).NotEmpty().WithMessage("Отсутствует валюта транзакции")
                .Must(type => Enum.TryParse(type, out CurrencyCode _))
                .WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(t => t.Description).NotEmpty().WithMessage("Отсутствует описание транзакции");

            RuleFor(t => t.DateTime).NotEmpty().WithMessage("Отсутствует дата и время отправки транзакции");

            RuleFor(t => t).Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.FromAccountId).Result;

                    if (accountDto != null)
                        return accountDto.CloseDate == null;

                    return true;
                }).WithMessage("Счёт, с которого происходит списание, закрыт")
                .Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.FromAccountId).Result;

                    if (accountDto != null)
                        return DateOnly.FromDateTime(t.DateTime) >= accountDto.OpenDate;

                    return true;
                }).WithMessage("Дата отправки транзакции не может быть раньше даты открытия счёта, с которого происходит списание");

            RuleFor(t => t).Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.ToAccountId).Result;

                    if (accountDto != null)
                        return accountDto.CloseDate == null;

                    return true;
                }).WithMessage("Счёт, на который происходит зачисление, закрыт")
                .Must(t =>
                {
                    AccountDto? accountDto = accountService.FindById(t.ToAccountId).Result;

                    if (accountDto != null)
                        return DateOnly.FromDateTime(t.DateTime) >= accountDto.OpenDate;

                    return true;
                }).WithMessage("Дата отправки транзакции не может быть раньше даты открытия счёта, на который происходит зачисление");
        }
    }
}