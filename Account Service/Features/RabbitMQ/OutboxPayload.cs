using JetBrains.Annotations;

namespace Account_Service.Features.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="meta"></param>
    public abstract class OutboxPayload(
        Guid eventId,
        DateTime occurredAt,
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
        public MessagePayload? Payload { get; }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Meta Meta { get; set; } = meta;
    }
}