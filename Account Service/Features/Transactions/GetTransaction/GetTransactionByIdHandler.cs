using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Transactions.GetTransaction
// ReSharper disable once ArrangeNamespaceBody
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
            var transaction = await _transactionsRepository.FindById(request.Id);

            return transaction != null ? TransactionMappers.MapToDto(transaction) : null;
        }
    }
}