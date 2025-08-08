using Account_Service.Features.Transactions;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public TransactionsRepository(ApplicationContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Transaction?> FindById(Guid id)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Transactions.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<Transaction>> FindAll()
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Transactions.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Transaction?> Save(Transaction entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
                await db.Transactions.AddAsync(entity);
                await db.SaveChangesAsync();

                return entity;
            }
            else
            {
                await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
                db.Transactions.Update(entity);
                await db.SaveChangesAsync();

                return entity;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            Transaction? transaction = await db.Transactions.FindAsync(id);

            if (transaction != null)
            {
                db.Transactions.Remove(transaction);

                return true;
            }

            return false;
        }
    }
}