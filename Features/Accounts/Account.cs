using Account_Service.Features.Transactions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Account_Service.Features.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ownerId"></param>
    /// <param name="type"></param>
    /// <param name="currency"></param>
    /// <param name="balance"></param>
    /// <param name="interestRate"></param>
    /// <param name="openDate"></param>
    /// <param name="closeDate"></param>
    public class Account(
        Guid id,
        Guid ownerId,
        AccountType type,
        CurrencyCode currency,
        decimal balance,
        decimal? interestRate,
        DateOnly openDate,
        DateOnly? closeDate)
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
        [Column("ownerId")]
        public Guid OwnerId { get; set; } = ownerId;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("type")]
        public AccountType Type { get; set; } = type;

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
        [Column("balance")]
        public decimal Balance { get; set; } = balance;

        /// <summary>
        /// 
        /// </summary>
        [Column("interestRate")]
        public decimal? InterestRate { get; set; } = interestRate;

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [Column("openDate")]
        public DateOnly OpenDate { get; set; } = openDate;

        /// <summary>
        /// 
        /// </summary>
        [Column("closeDate")]
        public DateOnly? CloseDate { get; set; } = closeDate;

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        /// <inheritdoc />
        public Account(Guid id, Account account) : this(id, account.OwnerId, account.Type, account.Currency, account.Balance, account.InterestRate, account.OpenDate, account.CloseDate)
        {
        }
    }
}