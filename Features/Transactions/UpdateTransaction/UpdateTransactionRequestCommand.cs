using System.Text.Json.Serialization;
using Account_Service.Features.Accounts;
using MediatR;

namespace Account_Service.Features.Transactions.UpdateTransaction
{
    /// <inheritdoc />
    public class UpdateTransactionRequestCommand(Guid id, Guid accountId, Guid counterpartyAccountId, decimal sum, string currency, string type, string description, DateTime dateTime) : IRequest<TransactionDto>
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore] public Guid Id { get; } = id;
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
        public string Currency { get; set; } = currency;
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; } = type;
        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; } = description;
        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTime { get; set; } = dateTime;
    }
}