using Account_Service.Features.Accounts;
using Account_Service.Infrastructure.Mappers;
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
        /// <param name="accountService"></param>
        public AddTransferTransactionsHandler(ITransactionsRepository transactionsRepository, IAccountService accountService)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(AddTransferTransactionsRequestCommand requestCommand,
            CancellationToken cancellationToken)
        {
            Transaction? transactionFrom = await _transactionsRepository.MakeTransfer(requestCommand.FromAccountId,
                requestCommand.ToAccountId, requestCommand);
            return transactionFrom != null ? TransactionMappers.MapToDto(transactionFrom) : null;
        }
    }
}