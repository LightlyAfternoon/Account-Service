using System.Text.Json.Serialization;
using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.Antifraud.UnblockAccount.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="eventId"></param>
    /// <param name="occurredAt"></param>
    /// <param name="clientId"></param>
    /// <param name="meta"></param>
    public class ClientUnblocked(Guid eventId, DateTime occurredAt, Guid clientId, Meta meta)
        : OutboxPayload(eventId, occurredAt, meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public new ClientUnblockedPayload Payload { get; } = new(clientId);

        /// <inheritdoc />
        [JsonConstructor]
        public ClientUnblocked(Guid eventId, DateTime occurredAt, ClientUnblockedPayload payload, Meta meta)
            : this(eventId, occurredAt, payload.ClientId, meta)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="clientId"></param>
    public class ClientUnblockedPayload(Guid clientId) : MessagePayload
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid ClientId { get; } = clientId;
    }
}