using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.RabbitMQ
{
    /// <inheritdoc />
    public interface IOutboxRepository : IRepository<Outbox>;
}