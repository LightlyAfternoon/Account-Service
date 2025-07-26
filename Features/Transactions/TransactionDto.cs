using Account_Service.Features.Accounts;
using System.Text.Json.Serialization;

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
    [method: JsonConstructor]
    public class TransactionDto(
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
        public Guid Id { get; } = id;
        /// <summary>
        /// 
        /// </summary>
        public Guid AccountId { get; set; } = accountId;
        /// <summary>
        /// 
        /// </summary>
        public Guid CounterpartyAccountId { get; set; } = counterpartyAccountId;
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

        /// <inheritdoc />
        public TransactionDto(Guid id, TransactionDto transactionDto) : this(id, transactionDto.AccountId, transactionDto.CounterpartyAccountId, transactionDto.Sum, transactionDto.Currency, transactionDto.Type, transactionDto.Description, transactionDto.DateTime)
        {
        }
    }
}