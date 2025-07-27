using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Transactions.TransactionsList
{
    /// <inheritdoc />
    public class GetTransactionsListHandler : IRequestHandler<GetTransactionsListRequestCommand, List<TransactionDto>>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public GetTransactionsListHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<List<TransactionDto>> Handle(GetTransactionsListRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _transactionsRepository.FindAll()).Select(TransactionMappers.MapToDto).ToList();
        }
    }
}