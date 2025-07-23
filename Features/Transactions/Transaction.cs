using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Accounts
{
    public class Transaction
    {
        [Required]
        [Column("id")]
        public Guid Id { get; set; }
        [Required]
        [Column("accountId")]
        public Account Account { get; set; }
        [Required]
        [Column("counterpartyAccountId")]
        public Account CounterpartyAccount { get; set; }
        [Required]
        [Column("sum")]
        public decimal Sum { get; set; }
        [Required]
        [Column("currency")]
        public CurrencyCode Currency { get; set; }
        [Required]
        [Column("type")]
        public AccountType Type { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }
        [Column("dateTime")]
        public DateTime DateTime { get; set; }
    }
}