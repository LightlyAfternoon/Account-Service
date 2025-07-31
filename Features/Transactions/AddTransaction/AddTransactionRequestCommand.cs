using MediatR;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <summary>
    /// Класс для запроса добавления транзакции по счёту
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="sum"></param>
    /// <param name="currency"></param>
    /// <param name="type"></param>
    /// <param name="description"></param>
    /// <param name="dateTime"></param>
    public class AddTransactionRequestCommand(Guid accountId, decimal sum, string currency, string type, string description, DateTime dateTime) : IRequest<TransactionDto>
    {
        /// <summary>
        /// Id аккаунта
        /// </summary>
        public Guid AccountId { get; set; } = accountId;
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
    }
}