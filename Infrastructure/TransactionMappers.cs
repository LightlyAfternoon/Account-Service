using Account_Service.Features.Accounts;
using Account_Service.Features.Transactions;

namespace Account_Service.Infrastructure
{
    public class TransactionMappers
    {
        public static TransactionDto MapToDto(Transaction transaction) => new(transaction.Id)
        {
            Account = new Account(transaction.Account.Id, transaction.Account),
            CounterpartyAccount = new Account(transaction.Account.Id, transaction.Account),
            Sum = transaction.Sum,
            Currency = transaction.Currency,
            Type = transaction.Type,
            Description = transaction.Description,
            DateTime = transaction.DateTime
        };

        public static Transaction MapToEntity(TransactionDto transactionDto) => new(transactionDto.Id)
        {
            Account = new Account(transactionDto.Account.Id, transactionDto.Account),
            CounterpartyAccount = new Account(transactionDto.Account.Id, transactionDto.Account),
            Sum = transactionDto.Sum,
            Currency = transactionDto.Currency,
            Type = transactionDto.Type,
            Description = transactionDto.Description,
            DateTime = transactionDto.DateTime
        };
    }
}