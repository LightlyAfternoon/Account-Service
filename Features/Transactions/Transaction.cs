using Account_Service.Features.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Transactions
{
    public class Transaction
    {
        [Required]
        [Column("id")]
        public Guid Id { get; }
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
        public TransactionType Type { get; set; }
        [Required]
        [Column("description")]
        public string Description { get; set; }
        [Column("dateTime")]
        public DateTime DateTime { get; set; }

        public Transaction(Guid id)
        {
            Id = id;
        }

        public Transaction(Guid id, Transaction transaction)
        {
            Id = id;
            Account = new Account(transaction.Account.Id, transaction.Account);
            CounterpartyAccount = new Account(transaction.CounterpartyAccount.Id, transaction.CounterpartyAccount);
            Sum = transaction.Sum;
            Currency = transaction.Currency;
            Type = transaction.Type;
            Description = transaction.Description;
            DateTime = transaction.DateTime;
        }

        public Transaction(Guid id, Account account, Account counterpartyAccount, decimal sum, CurrencyCode currency, TransactionType type, string description, DateTime dateTime) : this(id)
        {
            Id = id;
            Account = new Account(account.Id, account);
            CounterpartyAccount = new Account(counterpartyAccount.Id, counterpartyAccount);
            Sum = sum;
            Currency = currency;
            Type = type;
            Description = description;
            DateTime = dateTime;
        }
    }
}