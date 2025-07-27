using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.Transactions
{
    /// <inheritdoc />
    public interface ITransactionsRepository : IRepository<Transaction>;
}