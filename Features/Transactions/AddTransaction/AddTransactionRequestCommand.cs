using Account_Service.Features.Accounts;
using MediatR;

namespace Account_Service.Features.Transactions.AddTransaction
{
    public class AddTransactionRequestCommand :IRequest<TransactionDto>
    {
        public Account Account { get; set; }
        public Account CounterpartyAccount { get; set; }
        public decimal Sum { get; set; }
        public CurrencyCode Currency { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }
    }
}