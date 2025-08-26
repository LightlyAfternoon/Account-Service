using Account_Service.Features.Accounts;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
// ReSharper disable once ArrangeNamespaceBody
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
        public async Task<Account?> Save(Account entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
                await _context.Accounts.AddAsync(entity, cancellationToken);
            else
                _context.Accounts.Update(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            var account = await FindById(id);

            if (account == null)
                return false;

            _context.Accounts.Remove(account);

            return true;

        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAllByOwnerId(Guid ownerId)
        {
            return await _context.Accounts.Where(a => a.OwnerId.Equals(ownerId))
                .Include(a => a.Transactions)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<List<Account>> AccrueInterestForAllOpenedAccounts(CancellationToken cancellationToken)
        {
            var openedAccounts = await _context.Accounts.Where(a => (a.CloseDate == null
                                                                     || a.CloseDate >=
                                                                     DateOnly.FromDateTime(DateTime.UtcNow))
                                                                    && (a.Type.Equals(AccountType.Deposit) ||
                                                                        a.Type.Equals(AccountType.Credit)))
                .ToListAsync(cancellationToken);
            foreach (var account in openedAccounts)
            {
                await _context.Database.ExecuteSqlRawAsync("CALL accrue_interest({0})", account.Id);
            }

            return openedAccounts;
        }

        /// <inheritdoc />
        public async Task<List<Account>> FrozeAllUserAccounts(Guid ownerId, CancellationToken cancellationToken)
        {
            var allUserAccounts = await FindAllByOwnerId(ownerId);

            foreach (var account in allUserAccounts)
            {
                account.Frozen = true;

                _context.Accounts.Update(account);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return allUserAccounts;
        }

        /// <inheritdoc />
        public async Task<List<Account>> UnfrozeAllUserAccounts(Guid ownerId, CancellationToken cancellationToken)
        {
            var allUserAccounts = await FindAllByOwnerId(ownerId);

            foreach (var account in allUserAccounts)
            {
                account.Frozen = false;

                _context.Accounts.Update(account);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return allUserAccounts;
        }
    }
}