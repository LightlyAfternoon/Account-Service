using MediatR;

namespace Account_Service.Features.Transactions.DeleteTransaction
{
    public record DeleteTransactionRequestCommand(Guid Id) : IRequest<bool>;
}