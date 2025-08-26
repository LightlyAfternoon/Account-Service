using Account_Service.Features.Users;
using FluentValidation;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    /// <inheritdoc />
    [UsedImplicitly]
    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequestCommand>
    {
        /// <inheritdoc />
        public UpdateAccountRequestValidator(IUsersService usersService)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(a => a.Id).NotEmpty().WithMessage("Отсутствует id счёта");

            RuleFor(a => a.OwnerId).NotEmpty().WithMessage("Отсутствует id владельца счёта")
                .Must(a => usersService.FindById(a).Result != null).WithMessage("Владельца с данным id не существует");

            RuleFor(a => a.Type).NotEmpty().WithMessage("Отсутствует тип счёта")
                .Must(type => Enum.TryParse(type, out AccountType _)).WithMessage("Данный тип счёта не существует");

            RuleFor(a => a.Currency).NotEmpty().WithMessage("Отсутствует валюта счёта")
                .Must(type => Enum.TryParse(type, out CurrencyCode _)).WithMessage("Валюта с данным кодом не поддерживается");

            RuleFor(a => a.Balance).GreaterThanOrEqualTo(0).WithMessage("Баланс счёта меньше 0");

            RuleFor(a => a.InterestRate).Empty()
                .When(a => a.Type.Equals(nameof(AccountType.Checking))).WithMessage("Для текущего счёта не может быть процентной ставки");

            RuleFor(a => a.OpenDate).NotEmpty().WithMessage("Отсутствует дата открытия счёта");

            RuleFor(a => a.CloseDate).GreaterThanOrEqualTo(a => a.OpenDate)
                .When(a => a.CloseDate != null).WithMessage("Дата закрытия счёта раньше даты открытия");
        }
    }
}