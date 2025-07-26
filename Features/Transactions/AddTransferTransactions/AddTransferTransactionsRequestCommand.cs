using System.Text.Json.Serialization;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    /// <inheritdoc />
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
        /// 
        /// </summary>
        [JsonIgnore] public Guid FromAccountId { get; set; } = fromAccountId;

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore] public Guid ToAccountId { get; set; } = toAccountId;

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
        public string Description { get; set; } = description;

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTime { get; set; } = dateTime;
    }
}