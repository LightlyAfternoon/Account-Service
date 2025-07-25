using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.UpdateTransaction
{
    public class UpdateTransactionHandler : IRequestHandler<UpdateTransactionRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public UpdateTransactionHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task<TransactionDto?> Handle(UpdateTransactionRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            TransactionDto dto = new TransactionDto(requestCommand.Id)
            {
                Account = requestCommand.Account,
                CounterpartyAccount = requestCommand.CounterpartyAccount,
                Sum = requestCommand.Sum,
                Currency = requestCommand.Currency,
                Type = requestCommand.Type,
                Description = requestCommand.Description,
                DateTime = requestCommand.DateTime
            };

            Transaction? transaction = await _transactionsRepository.Save(TransactionMappers.MapToEntity(dto));

            if (transaction != null)
            {
                return TransactionMappers.MapToDto(transaction);
            }

            return null;
        }
    }
}