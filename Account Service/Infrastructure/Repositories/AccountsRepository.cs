using Account_Service.Features.Accounts;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class AccountsRepository : IAccountsRepository
    {
        private readonly ApplicationContext _context;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public AccountsRepository(ApplicationContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Account?> FindById(Guid id)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Accounts.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAll()
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Accounts.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Account?> Save(Account entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
                await db.Accounts.AddAsync(entity);
                await db.SaveChangesAsync();

                return entity;
            }
            else
            {
                await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
                db.Accounts.Update(entity);
                await db.SaveChangesAsync();

                return entity;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            Account? account = await db.Accounts.FindAsync(id);

            if (account != null)
            {
                db.Accounts.Remove(account);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAllByOwnerId(Guid ownerId)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            return await db.Accounts.Where(a => a.OwnerId.Equals(ownerId)).ToListAsync();
        }

        /// <inheritdoc />
        public async Task AccrueInterestForAllOpenedAccounts()
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            foreach (Account account in await db.Accounts.Where(a => a.CloseDate == null
                                                                      || a.CloseDate >= DateOnly.FromDateTime(DateTime.Now)).ToListAsync())
            {
                db.Accounts.FromSqlRaw("CALL accrue_interest({0})", account.Id);
            }
        }
    }
}