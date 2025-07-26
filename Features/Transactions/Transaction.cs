using Account_Service.Features.Accounts;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Transactions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="accountId"></param>
    /// <param name="counterpartyAccountId"></param>
    /// <param name="sum"></param>
    /// <param name="currency"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    /// <param name="dateTime"></param>
    public class Transaction(
        Guid id,
        Guid accountId,
        Guid counterpartyAccountId,
        decimal sum,
        CurrencyCode currency,
        TransactionType type,
        string description,
        DateTime dateTime)
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("id")]
        public Guid Id { get; } = id;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("accountId")]
        public Guid AccountId { get; set; } = accountId;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("counterpartyAccountId")]
        public Guid CounterpartyAccountId { get; set; } = counterpartyAccountId;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("sum")]
        public decimal Sum { get; set; } = sum;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("currency")]
        public CurrencyCode Currency { get; set; } = currency;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("type")]
        public TransactionType Type { get; set; } = type;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("description")]
        public string Description { get; set; } = description;

        /// <summary>
        /// 
        /// </summary>
        [Column("dateTime")]
        public DateTime DateTime { get; set; } = dateTime;

        /// <inheritdoc />
        public Transaction(Guid id, Transaction transaction) : this(id, transaction.AccountId, transaction.CounterpartyAccountId, transaction.Sum, transaction.Currency, transaction.Type, transaction.Description, transaction.DateTime)
        {
        }
    }
}