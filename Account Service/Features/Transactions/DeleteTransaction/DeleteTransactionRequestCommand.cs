using MediatR;

namespace Account_Service.Features.Transactions.DeleteTransaction
{
    /// <inheritdoc />
    public record DeleteTransactionRequestCommand(Guid Id) : IRequest<bool>;
}