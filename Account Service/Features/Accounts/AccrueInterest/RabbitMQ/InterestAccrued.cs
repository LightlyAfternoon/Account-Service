using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.AccrueInterest.RabbitMQ
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
    public class InterestAccrued(Guid eventId, DateTime occurredAt, Guid accountId, DateTime periodFrom, DateTime periodTo, decimal amount, Meta meta)
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
        public InterestAccruedPayload Payload { get; } = new(accountId, periodFrom, periodTo, amount);
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Meta Meta { get; set; } = meta;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="accountId"></param>
    /// <param name="periodFrom"></param>
    /// <param name="periodTo"></param>
    /// <param name="amount"></param>
    public class InterestAccruedPayload(Guid accountId, DateTime periodFrom, DateTime periodTo, decimal amount)
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