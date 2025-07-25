using Account_Service.Infrastructure;

namespace Account_Service.Features.Accounts
{
    public interface IAccountsRepository : IRepository<Account>
    {
        Task<List<Account>> FindAllByOwnerId(Guid ownerId);
    }
}