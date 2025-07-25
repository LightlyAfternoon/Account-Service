using MediatR;

namespace Account_Service.Features.Transactions.GetAccountStatementOnPeriod
{
    public record GetAccountStatementOnPeriodRequestCommand(Guid AccountId, DateTime StartDate, DateTime EndDate) : IRequest<List<TransactionDto>>;
}