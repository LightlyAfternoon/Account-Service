using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransaction
{
    /// <inheritdoc />
    public class AddTransactionHandler : IRequestHandler<AddTransactionRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public AddTransactionHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransactionRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            TransactionDto dto = new TransactionDto(id: Guid.Empty,
                accountId: requestCommand.AccountId,
                counterpartyAccountId: Guid.Empty,
                sum: requestCommand.Sum,
                currency: requestCommand.Currency,
                type: requestCommand.Type,
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