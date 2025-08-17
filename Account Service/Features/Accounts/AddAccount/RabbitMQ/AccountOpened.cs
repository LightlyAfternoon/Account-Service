using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.AddAccount.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="ownerId"></param>
    /// <param name="currency"></param>
    /// <param name="type"></param>
    /// <param name="meta"></param>
    public class AccountOpened(Guid eventId, DateTime occurredAt, Guid ownerId, string currency, string type, Meta meta)
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
        public AccountOpenedPayload Payload { get; } = new(ownerId, currency, type);
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
    /// <param name="currency"></param>
    /// <param name="type"></param>
    public class AccountOpenedPayload(Guid ownerId, string currency, string type)
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
        public string Currency { get; } = currency;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public string Type { get; } = type;
    }
}