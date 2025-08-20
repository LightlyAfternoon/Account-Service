using MediatR;

namespace Account_Service.Features.Transactions.DeleteTransaction
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record DeleteTransactionRequestCommand(Guid Id) : IRequest<bool>;
}