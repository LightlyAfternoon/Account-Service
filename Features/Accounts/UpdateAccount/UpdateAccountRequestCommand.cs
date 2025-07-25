using MediatR;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    public class UpdateAccountRequestCommand(Guid id) : IRequest<AccountDto>
    {
        public Guid Id { get; } = id;
        public User Owner { get; set; }
        public AccountType Type { get; set; }
        public CurrencyCode Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateOnly OpenDate { get; set; }
        public DateOnly? CloseDate { get; set; }
    }
}