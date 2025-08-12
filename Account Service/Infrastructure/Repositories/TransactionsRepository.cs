using Account_Service.Features.Accounts;
using Account_Service.Features.Transactions;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;
using System.Data;

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
            return await _context.Transactions.FindAsync(id);
        }

        /// <inheritdoc />
        public async Task<List<Transaction>> FindAll()
        {
            return await _context.Transactions.ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Transaction?> Save(Transaction entity)
        {
            if (entity.Id == Guid.Empty)
            {
                await _context.Transactions.AddAsync(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
            else
            {
                _context.Transactions.Update(entity);
                await _context.SaveChangesAsync();

                return entity;
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            Transaction? transaction = await _context.Transactions.FindAsync(id);

            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }

        /// <inheritdoc />
        public async Task<Transaction?> MakeTransfer(Guid fromAccountId, Guid toAccountId,
            AddTransferTransactionsRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            using var transaction =
                _context.Database.BeginTransactionAsync(IsolationLevel.Serializable, cancellationToken);
            try
            {
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

                await Save(transactionFrom);
                await Save(transactionTo);

                if (accountFrom != null)
                    await _accountsRepository.Save(accountFrom);
                if (accountTo != null)
                    await _accountsRepository.Save(accountTo);

                await (await transaction).CommitAsync(cancellationToken);

                return transactionFrom;
            }
            catch
            {
                await (await transaction).RollbackAsync(cancellationToken);

                throw new DbUpdateConcurrencyException();
            }
        }
    }
}