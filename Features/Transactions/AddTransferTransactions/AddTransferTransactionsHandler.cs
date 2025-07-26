using Account_Service.Features.Accounts;
using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    /// <inheritdoc />
    public class AddTransferTransactionsHandler : IRequestHandler<AddTransferTransactionsRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public AddTransferTransactionsHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransferTransactionsRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            Transaction? transactionFrom = new Transaction(id: Guid.Empty,
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

            await _transactionsRepository.Save(transactionTo);
            if ((transactionFrom = await _transactionsRepository.Save(transactionFrom)) != null)
            {
                return TransactionMappers.MapToDto(transactionFrom);
            }

            return null;
        }
    }
}