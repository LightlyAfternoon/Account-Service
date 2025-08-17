using System.Text.Json.Serialization;

namespace Account_Service.Features.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="messageId"></param>
    /// <param name="handler"></param>
    /// <param name="payload"></param>
    public class Inbox(Guid messageId, string handler, string payload)
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid MessageId { get; } = messageId;
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public DateTime? ProcessedAt { get; set; } = null;
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
