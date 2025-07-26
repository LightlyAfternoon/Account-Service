using Account_Service.Infrastructure;

namespace Account_Service.Features.Transactions
{
    /// <inheritdoc />
    public interface ITransactionsRepository : IRepository<Transaction>;
}