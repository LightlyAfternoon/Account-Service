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
            return await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAll()
        {
            return await _context.Accounts
                .Include(a => a.Transactions)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Account?> Save(Account entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await _context.Accounts.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            else
            {
                _context.Accounts.Update(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            Account? account = await _context.Accounts.FindAsync(id);

            if (account != null)
            {
                _context.Accounts.Remove(account);

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAllByOwnerId(Guid ownerId)
        {
            return await _context.Accounts.Where(a => a.OwnerId.Equals(ownerId))
                .Include(a => a.Transactions)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task AccrueInterestForAllOpenedAccounts()
        {
            foreach (Account account in await _context.Accounts.Where(a => a.CloseDate == null
                                                                      || a.CloseDate >= DateOnly.FromDateTime(DateTime.Now)
                                                                      && (a.Type.Equals(AccountType.Deposit) || a.Type.Equals(AccountType.Credit))).ToListAsync())
            {
                _context.Accounts.FromSqlRaw("CALL accrue_interest({0})", account.Id);
            }
        }
    }
}