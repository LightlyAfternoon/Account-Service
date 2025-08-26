using MediatR;

namespace Account_Service.Features.Transactions.GetTransaction
{
    /// <inheritdoc />
    public record GetTransactionByIdRequestCommand(Guid Id) : IRequest<TransactionDto>;
}