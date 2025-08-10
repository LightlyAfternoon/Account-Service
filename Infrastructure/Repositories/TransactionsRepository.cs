using System.Data;
using Account_Service.Features.Accounts;
using Account_Service.Features.Transactions;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
{
    /// <inheritdoc />
    public class TransactionsRepository : ITransactionsRepository
    {
        private readonly ApplicationContext _context;

        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="accountsRepository"></param>
        public TransactionsRepository(ApplicationContext context, IAccountsRepository accountsRepository)
        {
            _context = context;
            _accountsRepository = accountsRepository;
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

        /// <inheritdoc />
        public async Task<Transaction?> MakeTransfer(Guid fromAccountId, Guid toAccountId,
            AddTransferTransactionsRequestCommand requestCommand)
        {
            await using ApplicationContext db = new ApplicationContext(_context.ConnectionString);
            using var transaction = db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
            Transaction transactionFrom = new Transaction(id: Guid.Empty,
                accountId: requestCommand.FromAccountId,
                counterpartyAccountId: requestCommand.ToAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: TransactionType.Credit,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            Transaction transactionTo = new Transaction(id: Guid.Empty,
                accountId: requestCommand.ToAccountId,
                counterpartyAccountId: requestCommand.FromAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: TransactionType.Debit,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            Account? accountFrom = await _accountsRepository.FindById(requestCommand.FromAccountId);
            Account? accountTo = await _accountsRepository.FindById(requestCommand.ToAccountId);

            if (accountFrom != null)
                accountFrom.Balance -= requestCommand.Sum;
            if (accountTo != null)
                accountTo.Balance += requestCommand.Sum;

            await Save(transactionTo);
            await Save(transactionFrom);

            if (accountFrom != null)
                accountFrom = await _accountsRepository.Save(accountFrom);
            if (accountTo != null)
                accountTo = await _accountsRepository.Save(accountTo);

            Account? fixedAccountFrom = await _accountsRepository.FindById(requestCommand.FromAccountId);
            Account? fixedAccountTo = await _accountsRepository.FindById(requestCommand.ToAccountId);
            if (accountFrom != null && accountTo != null && fixedAccountFrom != null && fixedAccountTo != null
                && (fixedAccountFrom.Balance - requestCommand.Sum != accountFrom.Balance ||
                    fixedAccountTo.Balance + requestCommand.Sum != accountTo.Balance))
            {
                await (await transaction).RollbackAsync();

                return null;
            }
            else
            {
                await (await transaction).CommitAsync();

                return transactionFrom;
            }
        }
    }
}