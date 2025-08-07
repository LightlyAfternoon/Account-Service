using System.Text.Json.Serialization;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    /// <summary>
    /// Класс для запроса выполнения перевода между счетами (внутри банка)
    /// </summary>
    /// <param name="fromAccountId"></param>
    /// <param name="toAccountId"></param>
    /// <param name="sum"></param>
    /// <param name="currency"></param>
    /// <param name="description"></param>
    /// <param name="dateTime"></param>
    public class AddTransferTransactionsRequestCommand(
        Guid fromAccountId,
        Guid toAccountId,
        decimal sum,
        string currency,
        string description,
        DateTime dateTime)
        : IRequest<TransactionDto>
    {
        /// <summary>
        /// Id счёта, с которого происходит списание
        /// </summary>
        [JsonIgnore] public Guid FromAccountId { get; set; } = fromAccountId;

        /// <summary>
        /// Id счёта, на который происходит зачисление
        /// </summary>
        [JsonIgnore] public Guid ToAccountId { get; set; } = toAccountId;

        /// <summary>
        /// Сумма транзакции
        /// </summary>
        public decimal Sum { get; set; } = sum;

        /// <summary>
        /// Тип валюты
        /// </summary>
        public string Currency { get; set; } = currency;

        /// <summary>
        /// Описание транзакции
        /// </summary>
        public string Description { get; set; } = description;

        /// <summary>
        /// Дата/время проведения транзакции
        /// </summary>
        public DateTime DateTime { get; set; } = dateTime;
    }
}