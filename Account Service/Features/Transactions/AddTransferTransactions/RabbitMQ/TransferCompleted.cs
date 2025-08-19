using System.Text.Json.Serialization;
using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Transactions.AddTransferTransactions.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="sourceAccountId"></param>
    /// <param name="destinationAccountId"></param>
    /// <param name="amount"></param>
    /// <param name="currency"></param>
    /// <param name="transferId"></param>
    /// <param name="meta"></param>
    public class TransferCompleted(
        Guid eventId,
        DateTime occurredAt,
        Guid sourceAccountId,
        Guid destinationAccountId,
        decimal amount,
        string currency,
        Guid transferId,
        Meta meta) : OutboxPayload(eventId, occurredAt, meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public new TransferCompletedPayload Payload { get; } =
            new(sourceAccountId, destinationAccountId, amount, transferId, currency);

        /// <inheritdoc />
        [JsonConstructor]
        public TransferCompleted(Guid eventId, DateTime occurredAt, TransferCompletedPayload payload, Meta meta)
            : this(eventId, occurredAt, payload.SourceAccountId, payload.DestinationAccountId, payload.Amount,
                payload.Currency, payload.TransferId, meta)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sourceAccountId"></param>
    /// <param name="destinationAccountId"></param>
    /// <param name="amount"></param>
    /// <param name="transferId"></param>
    /// <param name="currency"></param>
    public class TransferCompletedPayload(
        Guid sourceAccountId,
        Guid destinationAccountId,
        decimal amount,
        Guid transferId,
        string currency) : MessagePayload

    {
    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public Guid SourceAccountId { get; } = sourceAccountId;

    /// <summary>
    /// 
    /// </summary>
    [UsedImplicitly]
    public Guid DestinationAccountId { get; } = destinationAccountId;

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
    public Guid TransferId { get; } = transferId;
    }
}