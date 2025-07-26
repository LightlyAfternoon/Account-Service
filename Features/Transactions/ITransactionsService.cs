using Account_Service.Features.Transactions.AddTransaction;
using Account_Service.Features.Transactions.AddTransferTransactions;
using Account_Service.Features.Transactions.UpdateTransaction;

namespace Account_Service.Features.Transactions
{
    /// <summary>
    /// 
    /// </summary>
    public interface ITransactionsService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TransactionDto?> FindById(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<TransactionDto>> FindAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestCommand"></param>
        /// <returns></returns>
        Task<TransactionDto?> Add(AddTransactionRequestCommand requestCommand);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="requestCommand"></param>
        /// <returns></returns>
        Task<TransactionDto?> Update(Guid id, UpdateTransactionRequestCommand requestCommand);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById(Guid id);

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