using MediatR;

namespace Account_Service.Features.Accounts.AddAccount
{
    public class AddAccountRequestCommand :IRequest<AccountDto>
    {
        public User Owner { get; set; }
        public AccountType Type { get; set; }
        public CurrencyCode Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateOnly OpenDate { get; set; }
        public DateOnly? CloseDate { get; set; }
    }
}