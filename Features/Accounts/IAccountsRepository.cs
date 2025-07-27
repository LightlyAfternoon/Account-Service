using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.Accounts
{
    /// <inheritdoc />
    public interface IAccountsRepository : IRepository<Account>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <returns></returns>
        Task<List<Account>> FindAllByOwnerId(Guid ownerId);
    }
}