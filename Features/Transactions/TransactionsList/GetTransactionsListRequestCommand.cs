using MediatR;

namespace Account_Service.Features.Transactions.TransactionsList
{
    /// <inheritdoc />
    public record GetTransactionsListRequestCommand : IRequest<List<TransactionDto>>;
}