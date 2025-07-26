using Account_Service.Features.Accounts;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    /// <inheritdoc />
    public class AccountsRepository : IAccountsRepository
    {
        /// <inheritdoc />
        public async Task<Account?> FindById(Guid id)
        {
            return await AccountsStorage.Find(id);
        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAll()
        {
            return await AccountsStorage.FindAll();
        }

        /// <inheritdoc />
        public async Task<Account?> Save(Account entity)
        {
            if (entity.Id == Guid.Empty)
            {
                return await AccountsStorage.Add(entity);
            }
            else
            {
                return await AccountsStorage.Update(entity);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            return await AccountsStorage.Delete(id);
        }

        /// <inheritdoc />
        public async Task<List<Account>> FindAllByOwnerId(Guid ownerId)
        {
            return (await AccountsStorage.FindAll()).Where(a => a.OwnerId.Equals(ownerId)).ToList();
        }
    }
}