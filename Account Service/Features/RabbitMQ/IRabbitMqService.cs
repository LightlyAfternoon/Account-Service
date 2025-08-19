using RabbitMQ.Client;

namespace Account_Service.Features.RabbitMQ
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRabbitMqService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IConnection? Connect();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="outbox"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task Publish(Outbox outbox, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task PublishAllNonProcessedFromOutbox();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task PublishClientBlocked(Guid id, CancellationToken cancellationToken);
    }
}
