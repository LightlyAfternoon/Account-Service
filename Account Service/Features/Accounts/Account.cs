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
        public bool Frozen { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public uint RowVersion { get; set; }

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
            Frozen = account.Frozen;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            var account = obj as Account;

            if (account == null)
                return false;
            else
                return Id.Equals(account.Id) && OwnerId.Equals(account.OwnerId) && Type.Equals(account.Type)
                       && Currency.Equals(account.Currency) && Balance.Equals(account.Balance)
                       && InterestRate.Equals(account.InterestRate) && OpenDate.Equals(account.OpenDate)
                       && CloseDate.Equals(account.CloseDate) && Frozen.Equals(account.Frozen);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}