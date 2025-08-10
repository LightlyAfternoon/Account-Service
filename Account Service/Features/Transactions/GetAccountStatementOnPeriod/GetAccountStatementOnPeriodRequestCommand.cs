using MediatR;

namespace Account_Service.Features.Transactions.GetAccountStatementOnPeriod
{
    /// <inheritdoc />
    public record GetAccountStatementOnPeriodRequestCommand(Guid AccountId, DateTime StartDate, DateTime EndDate) : IRequest<List<TransactionDto>>;
}