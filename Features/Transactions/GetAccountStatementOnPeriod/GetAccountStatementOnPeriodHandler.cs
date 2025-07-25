using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Transactions.GetAccountStatementOnPeriod
{
    public class GetAccountStatementOnPeriodHandler : IRequestHandler<GetAccountStatementOnPeriodRequestCommand, List<TransactionDto>>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        public GetAccountStatementOnPeriodHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        public async Task<List<TransactionDto>> Handle(GetAccountStatementOnPeriodRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _transactionsRepository.FindAll()).Where(t => t.Account.Id.Equals(requestCommand.AccountId) && t.DateTime >= requestCommand.StartDate && t.DateTime <= requestCommand.EndDate).Select(TransactionMappers.MapToDto).ToList();
        }
    }
}