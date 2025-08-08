using Account_Service.Features.Transactions;
using Account_Service.Features.Users;

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
        public Guid Id { get; } = id;

        /// <summary>
        /// 
        /// </summary>
        public Guid OwnerId { get; set; } = ownerId;

        /// <summary>
        /// 
        /// </summary>
        public AccountType Type { get; set; } = type;

        /// <summary>
        /// 
        /// </summary>
        public CurrencyCode Currency { get; set; } = currency;

        /// <summary>
        /// 
        /// </summary>
        public decimal Balance { get; set; } = balance;

        /// <summary>
        /// 
        /// </summary>
        public decimal? InterestRate { get; set; } = interestRate;

        /// <summary>
        /// 
        /// </summary>
        public DateOnly OpenDate { get; set; } = openDate;

        /// <summary>
        /// 
        /// </summary>
        public DateOnly? CloseDate { get; set; } = closeDate;

        /// <summary>
        /// 
        /// </summary>
        public User? Owner { get; set; }

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