using MediatR;

namespace Account_Service.Features.Transactions.GetAccountStatementOnPeriod
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record GetAccountStatementOnPeriodRequestCommand(Guid AccountId, DateTime StartDate, DateTime EndDate) : IRequest<List<TransactionDto>>;
}