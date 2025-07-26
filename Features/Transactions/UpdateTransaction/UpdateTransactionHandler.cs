using Account_Service.Features.Accounts;
using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.UpdateTransaction
{
    /// <inheritdoc />
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public UpdateTransactionHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(UpdateTransactionRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            TransactionDto dto = new TransactionDto(id: requestCommand.Id,
                accountId: requestCommand.AccountId,
                counterpartyAccountId: requestCommand.CounterpartyAccountId,
                sum: requestCommand.Sum,
                currency: Enum.Parse<CurrencyCode>(requestCommand.Currency),
                type: Enum.Parse<TransactionType>(requestCommand.Type),
                description: requestCommand.Description,
                dateTime: requestCommand.DateTime);

            Transaction? transaction = await _transactionsRepository.Save(TransactionMappers.MapToEntity(dto));

            if (transaction != null)
            {
                return TransactionMappers.MapToDto(transaction);
            }

            return null;
        }
    }
}