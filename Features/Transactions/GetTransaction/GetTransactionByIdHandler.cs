using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.GetTransaction
{
    /// <inheritdoc />
    public class GetTransactionByIdHandler : IRequestHandler<GetTransactionByIdRequestCommand, TransactionDto?>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public GetTransactionByIdHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<TransactionDto?> Handle(GetTransactionByIdRequestCommand request, CancellationToken cancellationToken)
        {
            Transaction? transaction = await _transactionsRepository.FindById(request.Id);

            if (transaction != null)
            {
                return TransactionMappers.MapToDto(transaction);
            }

            return null;
        }
    }
}