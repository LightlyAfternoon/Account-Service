using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Accounts
{
    public class AccountDto
    {
        public Guid Id { get; }
        public User Owner { get; set; }
        public AccountType Type { get; set; }
        public CurrencyCode Currency { get; set; }
        public decimal Balance { get; set; }
        public decimal? InterestRate { get; set; }
        public DateOnly OpenDate { get; set; }
        public DateOnly CloseDate { get; set; }

        public AccountDto(Guid id)
        {
            Id = id;
        }

        public AccountDto(Guid id, AccountDto accountDto)
        {
            Id = id;
            Owner = new User(accountDto.Owner.Id, accountDto.Owner);
            Type = accountDto.Type;
            Currency = accountDto.Currency;
            Balance = accountDto.Balance;
            InterestRate = accountDto.InterestRate;
            OpenDate = accountDto.OpenDate;
            CloseDate = accountDto.CloseDate;
        }
    }
}