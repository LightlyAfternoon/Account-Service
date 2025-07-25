using MediatR;

namespace Account_Service.Features.Transactions.TransactionsList
{
    public record GetTransactionsListRequestCommand : IRequest<List<TransactionDto>>;
}