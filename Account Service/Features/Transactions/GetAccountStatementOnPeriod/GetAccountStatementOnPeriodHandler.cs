using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Transactions.GetAccountStatementOnPeriod
{
    /// <inheritdoc />
    public class GetAccountStatementOnPeriodHandler : IRequestHandler<GetAccountStatementOnPeriodRequestCommand, List<TransactionDto>>
    {
        private readonly ITransactionsRepository _transactionsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionsRepository"></param>
        public GetAccountStatementOnPeriodHandler(ITransactionsRepository transactionsRepository)
        {
            _transactionsRepository = transactionsRepository;
        }

        /// <inheritdoc />
        public async Task<List<TransactionDto>> Handle(GetAccountStatementOnPeriodRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _transactionsRepository.FindAll()).Where(t => t.AccountId.Equals(requestCommand.AccountId) && t.DateTime >= requestCommand.StartDate && t.DateTime <= requestCommand.EndDate).Select(TransactionMappers.MapToDto).ToList();
        }
    }
}