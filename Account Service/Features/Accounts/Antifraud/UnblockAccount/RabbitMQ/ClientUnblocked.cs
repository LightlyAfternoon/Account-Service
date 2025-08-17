using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.Antifraud.UnblockAccount.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="clientId"></param>
    /// <param name="meta"></param>
    public class ClientUnblocked(Guid eventId, DateTime occurredAt, Guid clientId, Meta meta)
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
        public ClientUnblockedPayload Payload { get; } = new(clientId);
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Meta Meta { get; set; } = meta;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    public class ClientUnblockedPayload(Guid clientId)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid ClientId { get; } = clientId;
    }
}