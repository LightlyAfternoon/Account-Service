using Account_Service.Features.RabbitMQ;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class OutboxRepository : IOutboxRepository
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public OutboxRepository(ApplicationContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Outbox?> FindById(Guid id)
        {
            return await _context.Outboxes.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<Outbox>> FindAll()
        {
            return await _context.Outboxes.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Outbox?> Save(Outbox entity, CancellationToken cancellationToken)
        {
            if (entity.MessageId == Guid.Empty)
                await _context.Outboxes.AddAsync(entity, cancellationToken);
            else
                _context.Outboxes.Update(entity);

            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            Outbox? outbox = await FindById(id);

            if (outbox != null)
            {
                _context.Outboxes.Remove(outbox);

                return true;
            }

            return false;
        }
    }
}