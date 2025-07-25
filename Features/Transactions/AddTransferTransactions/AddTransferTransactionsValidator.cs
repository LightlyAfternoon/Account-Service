﻿using Account_Service.Features.Accounts;
using FluentValidation;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    /// <inheritdoc />
    public class AddTransferTransactionsValidator : AbstractValidator<AddTransferTransactionsRequestCommand>
    {
        /// <inheritdoc />
        public AddTransferTransactionsValidator(IAccountService accountService)
        {
            RuleFor(t => t.FromAccountId).NotEmpty().WithMessage("Отсутствует id счёта, с которого происходит списание денег");

            RuleFor(t => t.ToAccountId).NotEmpty().WithMessage("Отсутствует id счёта, на который происходит зачисление денег");

            RuleFor(t => t.Sum).NotEmpty().WithMessage("Отсутствует сумма транзакции");

            RuleFor(t => t.Currency).NotEmpty().WithMessage("Отсутствует валюта транзакции");

            RuleFor(t => t.Currency).Must(type => Enum.TryParse(type, out CurrencyCode _)).WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(t => t.Description).NotEmpty().WithMessage("Отсутствует описание транзакции");

            RuleFor(t => t.DateTime).NotEmpty().WithMessage("Отсутствует дата и время отправки транзакции");

            RuleFor(t => accountService.FindById(t.FromAccountId)).NotEmpty().WithMessage("Счёт с данным id не существует");

            RuleFor(t => accountService.FindById(t.ToAccountId)).NotEmpty().WithMessage("Счёт с данным id не существует");

            RuleFor(t => t.FromAccountId).NotEqual(t => t.ToAccountId).WithMessage("Счёт, с которого, и счёт, на который отправляются деньги, не могут быть одним и тем же счётом");
        }
    }
}