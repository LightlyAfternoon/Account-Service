using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.GetClientCurrentAccountBalance;
using Account_Service.Features.Accounts.UpdateAccount;

namespace Account_Service.Features.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AccountDto?> FindById(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<List<AccountDto>> FindAll();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<AccountDto?> Add(AddAccountRequestCommand dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<AccountDto?> Update(Guid id, UpdateAccountRequestCommand dto);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteById(Guid id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<bool> ClientWithIdHasAnyAccount(Guid ownerId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<GetClientCurrentAccountBalanceResponse?> GetClientCurrentAccountBalance(Guid ownerId);
    }
}