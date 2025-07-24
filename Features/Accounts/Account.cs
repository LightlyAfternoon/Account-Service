using Account_Service.Features.Transactions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Accounts
{
    public class Account
    {
        [Required]
        [Column("id")]
        public Guid Id { get; }
        [Required]
        [Column("ownerId")]
        public User Owner { get; set; }
        [Required]
        [Column("type")]
        public AccountType Type { get; set; }
        [Required]
        [Column("currency")]
        public CurrencyCode Currency { get; set; }
        [Required]
        [Column("balance")]
        public decimal Balance { get; set; }
        [Column("interestRate")]
        public decimal? InterestRate { get; set; }
        [Required]
        [Column("openDate")]
        public DateOnly OpenDate { get; set; }
        [Column("closeDate")]
        public DateOnly CloseDate { get; set; }

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public Account(Guid id)
        {
            Id = id;
        }

        public Account(Guid id, Account account)
        {
            Id = id;
            Owner = new User(account.Owner.Id, account.Owner);
            Type = account.Type;
            Currency = account.Currency;
            Balance = account.Balance;
            InterestRate = account.InterestRate;
            OpenDate = account.OpenDate;
            CloseDate = account.CloseDate;
        }
    }
}