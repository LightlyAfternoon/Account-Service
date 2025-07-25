using Account_Service.Infrastructure;

namespace Account_Service.Features.Transactions
{
    public interface ITransactionsService : IService<TransactionDto>
    {
        Task<List<TransactionDto>> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate);
        Task<TransactionDto?> Transfer(Guid fromAccountId, Guid toAccountId, TransactionDto transactionDto);
    }
}