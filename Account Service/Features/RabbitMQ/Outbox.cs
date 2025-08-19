namespace Account_Service.Features.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="routingKey"></param>
    /// <param name="handler"></param>
    /// <param name="payload"></param>
    /// <param name="processedAt"></param>
    public class Outbox(Guid messageId, string routingKey, string handler, string payload, DateTime? processedAt = null)
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid MessageId { get; } = messageId;
        /// <summary>
        /// 
        /// </summary>
        public string RoutingKey { get; set; } = routingKey;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ProcessedAt { get; set; } = processedAt;
        /// <summary>
        /// 
        /// </summary>
        public string Handler { get; } = handler;
        /// <summary>
        /// 
        /// </summary>
        public string Payload { get; } = payload;
    }
}
