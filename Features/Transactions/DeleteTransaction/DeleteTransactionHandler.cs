using MediatR;

namespace Account_Service.Features.Transactions.DeleteTransaction
{
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionRequestCommand, bool>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public DeleteTransactionHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task<bool> Handle(DeleteTransactionRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return await _transactionsRepository.DeleteById(requestCommand.Id);
        }
    }
}