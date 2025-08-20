using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;

namespace Account_Service.Features.Transactions
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionsService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCommand"></param>
        /// <returns></returns>
        Task<TransactionDto?> Add(AddTransactionRequestCommand requestCommand);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountId"></param>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<List<TransactionDto>> GetAccountStatementOnPeriod(Guid accountId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromAccountId"></param>
        /// <param name="toAccountId"></param>
        /// <param name="requestCommand"></param>
        /// <returns></returns>
        Task<TransactionDto?> Transfer(Guid fromAccountId, Guid toAccountId, AddTransferTransactionsRequestCommand requestCommand);
    }
}