using System.Text.Json.Serialization;
using Account_Service.Features.RabbitMQ;
using JetBrains.Annotations;

namespace Account_Service.Features.Accounts.AddAccount.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
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
        : OutboxPayload(eventId, occurredAt, meta)
    {
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public new AccountOpenedPayload Payload { get; } = new(ownerId, currency, type);

        /// <inheritdoc />
        [JsonConstructor]
        public AccountOpened(Guid eventId, DateTime occurredAt, AccountOpenedPayload payload, Meta meta)
            : this(eventId, occurredAt, payload.OwnerId, payload.Currency, payload.Type, meta)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="currency"></param>
    /// <param name="type"></param>
    public class AccountOpenedPayload(Guid ownerId, string currency, string type) : MessagePayload
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