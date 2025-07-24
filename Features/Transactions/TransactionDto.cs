using Account_Service.Features.Accounts;

namespace Account_Service.Features.Transactions
{
    public class TransactionDto
    {
        public Guid Id { get; }
        public Account Account { get; set; }
        public Account CounterpartyAccount { get; set; }
        public decimal Sum { get; set; }
        public CurrencyCode Currency { get; set; }
        public TransactionType Type { get; set; }
        public string Description { get; set; }
        public DateTime DateTime { get; set; }

        public TransactionDto(Guid id)
        {
            Id = id;
        }

        public TransactionDto(Guid id, TransactionDto transactionDto)
        {
            Id = id;
            Account = new Account(transactionDto.Account.Id, transactionDto.Account);
            CounterpartyAccount = new Account(transactionDto.Account.Id, transactionDto.Account);
            Sum = transactionDto.Sum;
            Currency = transactionDto.Currency;
            Type = transactionDto.Type;
            Description = transactionDto.Description;
            DateTime = transactionDto.DateTime;
        }
    }
}