using System.Text.Json.Serialization;
using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.AccrueInterest.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="accountId"></param>
    /// <param name="periodFrom"></param>
    /// <param name="periodTo"></param>
    /// <param name="amount"></param>
    /// <param name="meta"></param>
    public class InterestAccrued(
        Guid eventId,
        DateTime occurredAt,
        Guid accountId,
        DateTime periodFrom,
        DateTime periodTo,
        decimal amount,
        Meta meta) : OutboxPayload(eventId, occurredAt, meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public new InterestAccruedPayload Payload { get; } = new(accountId, periodFrom, periodTo, amount);

        /// <inheritdoc />
        [JsonConstructor]
        public InterestAccrued(Guid eventId, DateTime occurredAt, InterestAccruedPayload payload, Meta meta)
            : this(eventId, occurredAt, payload.AccountId, payload.PeriodFrom, payload.PeriodTo, payload.Amount, meta)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="periodFrom"></param>
    /// <param name="periodTo"></param>
    /// <param name="amount"></param>
    public class InterestAccruedPayload(Guid accountId, DateTime periodFrom, DateTime periodTo, decimal amount) : MessagePayload
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid AccountId { get; } = accountId;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public DateTime PeriodFrom = periodFrom;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public DateTime PeriodTo = periodTo;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public decimal Amount { get; } = amount;
    }
}