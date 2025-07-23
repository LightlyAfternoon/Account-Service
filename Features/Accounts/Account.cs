using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Accounts
{
    public class Account
    {
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
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

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}