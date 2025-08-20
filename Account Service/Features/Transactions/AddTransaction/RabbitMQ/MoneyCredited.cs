using System.Text.Json.Serialization;
using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Transactions.AddTransaction.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
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
    public class MoneyCredited(
        Guid eventId,
        DateTime occurredAt,
        Guid ownerId,
        decimal amount,
        string currency,
        Guid operationId,
        Meta meta) : OutboxPayload(eventId, occurredAt, meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public new MoneyCreditedPayload Payload { get; } = new(ownerId, amount, currency, operationId);

        /// <inheritdoc />
        [JsonConstructor]
        public MoneyCredited(Guid eventId, DateTime occurredAt, MoneyCreditedPayload payload, Meta meta)
            : this(eventId, occurredAt, payload.OwnerId, payload.Amount, payload.Currency, payload.OperationId, meta)
        {
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <param name="operationId"></param>
    public class MoneyCreditedPayload(Guid ownerId, decimal amount, string currency, Guid operationId) : MessagePayload
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