using MediatR;

namespace Account_Service.Features.Transactions.GetTransaction
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record GetTransactionByIdRequestCommand(Guid Id) : IRequest<TransactionDto>;
}