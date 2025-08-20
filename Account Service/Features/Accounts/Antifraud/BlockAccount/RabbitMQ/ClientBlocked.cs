using System.Text.Json.Serialization;
using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.Antifraud.BlockAccount.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="clientId"></param>
    /// <param name="meta"></param>
    public class ClientBlocked(Guid eventId, DateTime occurredAt, Guid clientId, Meta meta)
        : OutboxPayload(eventId, occurredAt, meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public new ClientBlockedPayload Payload { get; } = new(clientId);

        /// <inheritdoc />
        [JsonConstructor]
        public ClientBlocked(Guid eventId, DateTime occurredAt, ClientBlockedPayload payload, Meta meta)
            : this(eventId, occurredAt, payload.ClientId, meta)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    public class ClientBlockedPayload(Guid clientId) : MessagePayload
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ClientId { get; } = clientId;
    }
}