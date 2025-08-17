using Account_Service.Features.Accounts;

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
        Guid? counterpartyAccountId,
        decimal sum,
        CurrencyCode currency,
        TransactionType type,
        string description,
        DateTime dateTime)
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; } = id;

        /// <summary>
        /// 
        /// </summary>
        public Guid AccountId { get; set; } = accountId;

        /// <summary>
        /// 
        /// </summary>
        public Guid? CounterpartyAccountId { get; set; } = counterpartyAccountId;

        /// <summary>
        /// 
        /// </summary>
        public decimal Sum { get; set; } = sum;

        /// <summary>
        /// 
        /// </summary>
        public CurrencyCode Currency { get; set; } = currency;

        /// <summary>
        /// 
        /// </summary>
        public TransactionType Type { get; set; } = type;

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; } = description;

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTime { get; set; } = dateTime;

        /// <summary>
        /// 
        /// </summary>
        public uint RowVersion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Account? Account { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Account? CounterpartyAccount { get; set; }

        /// <inheritdoc />
        public Transaction(Guid id, Transaction transaction) : this(id, transaction.AccountId,
            transaction.CounterpartyAccountId, transaction.Sum, transaction.Currency, transaction.Type,
            transaction.Description, transaction.DateTime)
        {
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            var transaction = obj as Transaction;

            if (transaction == null)
                return false;
            else
                return Id.Equals(transaction.Id) && AccountId.Equals(transaction.AccountId) &&
                       CounterpartyAccountId.Equals(transaction.CounterpartyAccountId) && Sum.Equals(transaction.Sum) &&
                       Currency.Equals(transaction.Currency) && Type.Equals(transaction.Type) &&
                       Description.Equals(transaction.Description) && DateTime.Equals(transaction.DateTime);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}