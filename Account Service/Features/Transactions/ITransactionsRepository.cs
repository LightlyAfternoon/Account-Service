using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.Transactions
{
    /// <inheritdoc />
    public interface ITransactionsRepository : IRepository<Transaction>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        /// <param name="requestCommand"></param>
        /// <returns></returns>
        Task<Transaction?> MakeTransfer(Guid fromAccountId, Guid toAccountId, AddTransferTransactionsRequestCommand requestCommand);
    }
}