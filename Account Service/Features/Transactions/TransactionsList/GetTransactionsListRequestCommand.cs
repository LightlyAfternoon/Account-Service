using MediatR;

namespace Account_Service.Features.Transactions.TransactionsList
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record GetTransactionsListRequestCommand : IRequest<List<TransactionDto>>;
}