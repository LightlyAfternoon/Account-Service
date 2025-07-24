using Account_Service.Infrastructure;

namespace Account_Service.Features.Transactions
{
    public interface ITransactionsRepository : IRepository<Transaction>
    {
    }
}