namespace Account_Service.Features.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="handler"></param>
    /// <param name="payload"></param>
    /// <param name="error"></param>
    /// <param name="receivedAt"></param>
    public class InboxDeadLetters(Guid messageId, string handler, string payload, string error, DateTime? receivedAt = null)
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid MessageId { get; } = messageId;
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ReceivedAt { get; set; } = receivedAt;
        /// <summary>
        /// 
        /// </summary>
        public string Handler { get; } = handler;
        /// <summary>
        /// 
        /// </summary>
        public string Payload { get; } = payload;
        /// <summary>
        /// 
        /// </summary>
        public string Error { get; } = error;

        /// <inheritdoc />
        public InboxDeadLetters(Inbox inbox, string error, DateTime? receivedAt = null) : this(inbox.MessageId,
            inbox.Handler, inbox.Payload, error, receivedAt)
        {
        }
    }
}