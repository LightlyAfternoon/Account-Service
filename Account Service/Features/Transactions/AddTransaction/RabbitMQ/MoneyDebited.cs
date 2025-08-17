using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Transactions.AddTransaction.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="ownerId"></param>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <param name="operationId"></param>
    /// <param name="meta"></param>
    public class MoneyDebited(
        Guid eventId,
        DateTime occurredAt,
        Guid ownerId,
        decimal amount,
        string currency,
        Guid operationId,
        Meta meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid EventId { get; } = eventId;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public DateTime OccurredAt { get; } = occurredAt;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public MoneyDebitedPayload Payload { get; } = new(ownerId, amount, currency, operationId);
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Meta Meta { get; set; } = meta;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <param name="operationId"></param>
    public class MoneyDebitedPayload(Guid ownerId, decimal amount, string currency, Guid operationId)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid OwnerId { get; } = ownerId;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public decimal Amount { get; } = amount;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public string Currency { get; } = currency;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid OperationId { get; } = operationId;
    }
}