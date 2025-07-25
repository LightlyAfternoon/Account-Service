using Account_Service.Features.Accounts;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransferTransactions
{
    public class AddTransferTransactionsRequestCommand(Guid fromAccountId, Guid toAccountId) : IRequest<TransactionDto>
    {
        public Guid FromAccountId { get; } = fromAccountId;
        public Guid ToAccountId { get; } = toAccountId;
        public decimal Sum { get; set; }
        public CurrencyCode Currency { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}