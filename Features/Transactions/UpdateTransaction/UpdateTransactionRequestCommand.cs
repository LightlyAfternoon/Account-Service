using Account_Service.Features.Accounts;
using MediatR;

namespace Account_Service.Features.Transactions.UpdateTransaction
{
    public class UpdateTransactionRequestCommand(Guid id) : IRequest<TransactionDto>
    {
        public Guid Id { get; } = id;
        public Account Account { get; set; }
        public Account CounterpartyAccount { get; set; }
        public decimal Sum { get; set; }
        public CurrencyCode Currency { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}