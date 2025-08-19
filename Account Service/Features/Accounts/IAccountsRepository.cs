using Account_Service.Infrastructure.Repositories;

namespace Account_Service.Features.Accounts
// ReSharper disable once ArrangeNamespaceBody
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Account>> AccrueInterestForAllOpenedAccounts(CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Account>> FrozeAllUserAccounts(Guid ownerId, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ownerId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Account>> UnfrozeAllUserAccounts(Guid ownerId, CancellationToken cancellationToken);
    }
}