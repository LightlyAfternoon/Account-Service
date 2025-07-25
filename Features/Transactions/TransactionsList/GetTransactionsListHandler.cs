using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.TransactionsList
{
    public class GetTransactionsListHandler : IRequestHandler<GetTransactionsListRequestCommand, List<TransactionDto>>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public GetTransactionsListHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task<List<TransactionDto>> Handle(GetTransactionsListRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _transactionsRepository.FindAll()).Select(TransactionMappers.MapToDto).ToList();
        }
    }
}