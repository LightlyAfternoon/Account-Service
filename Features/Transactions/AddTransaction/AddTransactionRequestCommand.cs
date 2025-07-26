using MediatR;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <inheritdoc />
    public class AddTransactionRequestCommand(Guid accountId, decimal sum, string currency, string type, string description, DateTime dateTime) : IRequest<TransactionDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid AccountId { get; set; } = accountId;
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