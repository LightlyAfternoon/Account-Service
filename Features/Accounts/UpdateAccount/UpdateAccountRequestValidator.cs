using Account_Service.Features.Users;
using FluentValidation;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    /// <inheritdoc />
    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequestCommand>
    {
        /// <inheritdoc />
        public UpdateAccountRequestValidator(IUsersService usersService)
        {
            RuleFor(a => a.Id).NotEmpty().WithMessage("Отсутствует id счёта");

            RuleFor(a => a.OwnerId).NotEmpty().WithMessage("Отсутствует id владельца счёта");

            RuleFor(a => a.Type).NotEmpty().WithMessage("Отсутствует тип счёта");

            RuleFor(a => a.Type).Must(type => Enum.TryParse(type, out AccountType _)).WithMessage("Данный тип счёта не существует");

            RuleFor(a => a.Currency).NotEmpty().WithMessage("Отсутствует валюта счёта");

            RuleFor(a => a.Currency).Must(type => Enum.TryParse(type, out CurrencyCode _)).WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(a => a.Balance).NotEmpty().WithMessage("Отсутствует баланс счёта");

            RuleFor(a => a.InterestRate).Empty()
                .When(a => Enum.Parse<AccountType>(a.Type).Equals(AccountType.Checking)).WithMessage("Для текущего счёта не может быть процентной ставки");
            
            RuleFor(a => a.OpenDate).NotEmpty().WithMessage("Отсутствует дата открытия счёта");

            RuleFor(a => a.CloseDate).GreaterThanOrEqualTo(a => a.OpenDate)
                .When(a => a.CloseDate != null).WithMessage("Дата закрытия счёта раньше даты открытия");

            RuleFor(a => usersService.FindById(a.OwnerId).Result).NotEmpty().WithMessage("Владельца с данным id не существует");
        }
    }
}