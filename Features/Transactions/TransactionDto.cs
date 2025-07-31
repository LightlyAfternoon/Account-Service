using System.Text.Json.Serialization;

namespace Account_Service.Features.Transactions
{
    /// <summary>
    /// DTO транзакции
    /// </summary>
    /// <param name="id">Id транзакции</param>
    /// <param name="accountId">Id счёта</param>
    /// <param name="counterpartyAccountId">Id счёта-контрагента</param>
    /// <param name="sum">Сумма транзакции</param>
    /// <param name="currency">Тип валюты</param>
    /// <param name="type">Тип транзакции</param>
    /// <param name="description">Описание транзакции</param>
    /// <param name="dateTime">Дата/время транзакции</param>
    [method: JsonConstructor]
    public class TransactionDto(
        Guid id,
        Guid accountId,
        Guid counterpartyAccountId,
        decimal sum,
        string currency,
        string type,
        string description,
        DateTime dateTime)
    {
        /// <summary>
        /// Id транзакции
        /// </summary>
        public Guid Id { get; } = id;
        /// <summary>
        /// Id счёта
        /// </summary>
        public Guid AccountId { get; set; } = accountId;
        /// <summary>
        /// Id счёта-контрагента
        /// </summary>
        public Guid CounterpartyAccountId { get; set; } = counterpartyAccountId;
        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal Sum { get; set; } = sum;
        /// <summary>
        /// Тип валюты
        /// </summary>
        public string Currency { get; set; } = currency;
        /// <summary>
        /// Тип транзакции
        /// </summary>
        public string Type { get; set; } = type;
        /// <summary>
        /// Описание транзакции
        /// </summary>
        public string Description { get; set; } = description;
        /// <summary>
        /// Дата/время проведения транзакции
        /// </summary>
        public DateTime DateTime { get; set; } = dateTime;

        /// <inheritdoc />
        public TransactionDto(Guid id, TransactionDto transactionDto) : this(id, transactionDto.AccountId, transactionDto.CounterpartyAccountId, transactionDto.Sum, transactionDto.Currency, transactionDto.Type, transactionDto.Description, transactionDto.DateTime)
        {
        }
    }
}