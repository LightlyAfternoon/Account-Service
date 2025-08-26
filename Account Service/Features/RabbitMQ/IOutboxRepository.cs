using Account_Service.Infrastructure.Repositories;
// ReSharper disable once ArrangeNamespaceBody

namespace Account_Service.Features.RabbitMQ
{
    /// <inheritdoc />
    public interface IOutboxRepository : IRepository<Outbox>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<Outbox>> FindAllNotProcessed();
    }
}