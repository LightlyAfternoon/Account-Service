using Account_Service.Features.Accounts;
using Account_Service.Infrastructure;

namespace Account_Service.Features.Transactions
{
    public class TransactionsService : ITransactionsService
    {
        private readonly ITransactionsRepository _transactionsRepository;
        private readonly IAccountsRepository _accountsRepository;

        public TransactionsService(ITransactionsRepository transactionsRepository, IAccountsRepository accountsRepository)
        {
            _transactionsRepository = transactionsRepository;
            _accountsRepository = accountsRepository;
        }

        public TransactionDto? FindById(Guid id)
        {
            Transaction? transaction = _transactionsRepository.FindById(id);

            return transaction != null ? TransactionMappers.MapToDto(transaction) : null;
        }

        public List<TransactionDto> FindAll()
        {
            return _transactionsRepository.FindAll().Select(TransactionMappers.MapToDto).ToList();
        }

        public TransactionDto? Add(TransactionDto dto)
        {
            dto = new TransactionDto(Guid.Empty, dto);

            Transaction? transaction = _transactionsRepository.Save(TransactionMappers.MapToEntity(dto));

            if (transaction != null)
            {
                return TransactionMappers.MapToDto(transaction);
            }

            return null;
        }

        public TransactionDto? Update(Guid id, TransactionDto dto)
        {
            dto = new TransactionDto(id, dto);

            Transaction? transaction = _transactionsRepository.Save(TransactionMappers.MapToEntity(dto));

            if (transaction != null)
            {
                return TransactionMappers.MapToDto(transaction);
            }

            return null;
        }

        public bool DeleteById(Guid id)
        {
            return _transactionsRepository.DeleteById(id);
        }

        public List<TransactionDto> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate)
        {
            return _transactionsRepository.FindAll().Where(t => t.Account.Id.Equals(accountId) && t.DateTime >= startDate && t.DateTime <= endDate).Select(TransactionMappers.MapToDto).ToList();
        }

        public TransactionDto? Transfer(Guid fromAccountId, Guid toAccountId, TransactionDto transactionDto)
        {
            Account? from = _accountsRepository.FindById(fromAccountId);
            Account? to = _accountsRepository.FindById(toAccountId);
            Transaction? transactionFrom = new Transaction(Guid.Empty, from, to, transactionDto.Sum, transactionDto.Currency,
                                                                    TransactionType.Credit, transactionDto.Description, transactionDto.DateTime);
            Transaction transactionTo = new Transaction(Guid.Empty, to, from, transactionDto.Sum, transactionDto.Currency,
                                                                TransactionType.Debit, transactionDto.Description, transactionDto.DateTime);

            _transactionsRepository.Save(transactionTo);
            if ((transactionFrom = _transactionsRepository.Save(transactionFrom)) != null)
            {
                return TransactionMappers.MapToDto(transactionFrom);
            }

            return null;
        }
    }
}