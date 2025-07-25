using Account_Service.Features.Accounts;
using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    public class AddTransferTransactionsHandler : IRequestHandler<AddTransferTransactionsRequestCommand, TransactionDto?>
    {
        private readonly IAccountsRepository _accountsRepository;
        private readonly ITransactionsRepository _transactionsRepository;

        public AddTransferTransactionsHandler(IAccountsRepository accountsRepository, ITransactionsRepository transactionsRepository)
        {
            _accountsRepository = accountsRepository;
            _transactionsRepository = transactionsRepository;
        }

        public async Task<TransactionDto?> Handle(AddTransferTransactionsRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            Account? from = await _accountsRepository.FindById(requestCommand.FromAccountId);
            Account? to = await _accountsRepository.FindById(requestCommand.ToAccountId);

            if (from != null && to != null)
            {
                Transaction? transactionFrom = new Transaction(Guid.Empty)
                {
                    Account = from,
                    CounterpartyAccount = to,
                    Sum = requestCommand.Sum,
                    Currency = requestCommand.Currency,
                    Type = TransactionType.Credit,
                    Description = requestCommand.Description,
                    DateTime = requestCommand.DateTime
                };

                Transaction transactionTo = new Transaction(Guid.Empty)
                {
                    Account = to,
                    CounterpartyAccount = from,
                    Sum = requestCommand.Sum,
                    Currency = requestCommand.Currency,
                    Type = TransactionType.Debit,
                    Description = requestCommand.Description,
                    DateTime = requestCommand.DateTime
                };

                await _transactionsRepository.Save(transactionTo);
                if ((transactionFrom = await _transactionsRepository.Save(transactionFrom)) != null)
                {
                    return TransactionMappers.MapToDto(transactionFrom);
                }
            }

            return null;
        }
    }
}