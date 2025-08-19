using Account_Service.Features.Accounts;
using Account_Service.Features.Transactions;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

namespace Account_Service.Infrastructure.Repositories
// ReSharper disable once ArrangeNamespaceBody
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
        public async Task<Transaction?> Save(Transaction entity, CancellationToken cancellationToken)
        {
            if (entity.Id == Guid.Empty)
            {
                await _context.Transactions.AddAsync(entity, cancellationToken);
            }
            else
            {
                _context.Transactions.Update(entity);
            }

            await _context.SaveChangesAsync(cancellationToken);
            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
                return false;

            _context.Transactions.Remove(transaction);
            await _context.SaveChangesAsync();

            return true;

        }

        /// <inheritdoc />
        public async Task<Transaction?> MakeTransfer(Guid fromAccountId, Guid toAccountId,
            AddTransferTransactionsRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            var transactionFrom = new Transaction(id: Guid.Empty,
                accountId: requestCommand.FromAccountId,
                counterpartyAccountId: requestCommand.ToAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: TransactionType.Debit,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            var transactionTo = new Transaction(id: Guid.Empty,
                accountId: requestCommand.ToAccountId,
                counterpartyAccountId: requestCommand.FromAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: TransactionType.Credit,
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            var accountFrom = await _accountsRepository.FindById(requestCommand.FromAccountId);
            var accountTo = await _accountsRepository.FindById(requestCommand.ToAccountId);

            if (accountFrom != null)
                accountFrom.Balance -= requestCommand.Sum;
            if (accountTo != null)
                accountTo.Balance += requestCommand.Sum;

            await Save(transactionFrom, cancellationToken);
            await Save(transactionTo, cancellationToken);

            if (accountFrom != null)
                await _accountsRepository.Save(accountFrom, cancellationToken);
            if (accountTo != null)
                await _accountsRepository.Save(accountTo, cancellationToken);

            return transactionFrom;
        }
    }
}