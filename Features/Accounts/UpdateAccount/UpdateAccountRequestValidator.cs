using FluentValidation;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    public class UpdateAccountRequestValidator : AbstractValidator<UpdateAccountRequestCommand>
    {
        public UpdateAccountRequestValidator()
        {
            RuleFor(a => a.Id).NotEmpty();

            RuleFor(a => a.Owner).NotEmpty();

            RuleFor(a => a.Type).NotEmpty();

            RuleFor(a => a.Currency).NotEmpty();

            RuleFor(a => a.Balance).NotEmpty();

            RuleFor(a => a.InterestRate).Empty()
                .When(a => a.Type.Equals(AccountType.Checking));
            
            RuleFor(a => a.OpenDate).NotEmpty();

            RuleFor(a => a.CloseDate).GreaterThanOrEqualTo(a => a.OpenDate)
                .When(a => a.CloseDate != null);
        }
    }
}