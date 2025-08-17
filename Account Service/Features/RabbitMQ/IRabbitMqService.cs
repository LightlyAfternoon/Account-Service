namespace Account_Service.Features.RabbitMQ
{
    /// <summary>
    /// 
    /// </summary>
    public interface IRabbitMqService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="outbox"></param>
        /// <returns></returns>
        Task Publish(Outbox outbox);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task PublishAllNonProcessedFromOutbox();
    }
}
