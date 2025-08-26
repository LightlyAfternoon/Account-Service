using MediatR;

namespace Account_Service.Features.Transactions.DeleteTransaction
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class DeleteTransactionHandler : IRequestHandler<DeleteTransactionRequestCommand, bool>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public DeleteTransactionHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteTransactionRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return await _transactionsRepository.DeleteById(requestCommand.Id);
        }
    }
}