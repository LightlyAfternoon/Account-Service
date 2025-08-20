using JetBrains.Annotations;

namespace Account_Service.Features.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    /// <param name="source"></param>
    /// <param name="correlationId"></param>
    /// <param name="causationId"></param>
    public class Meta(string version, string source, Guid correlationId, Guid causationId)
    {
        /// <summary>
        /// 
        /// </summary>
        public string Version { get; set; } = version;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public string Source { get; } = source;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid CorrelationId { get; } = correlationId;
        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly]
        public Guid CausationId { get; } = causationId;
    }
}
