using Account_Service.Features.Accounts;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    public class AccountsRepository : IAccountsRepository
    {
        public async Task<Account?> FindById(Guid id)
        {
            return await AccountsStorage.Find(id);
        }

        public async Task<List<Account>> FindAll()
        {
            return await AccountsStorage.FindAll();
        }

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

        public async Task<bool> DeleteById(Guid id)
        {
            return await AccountsStorage.Delete(id);
        }

        public async Task<List<Account>> FindAllByOwnerId(Guid ownerId)
        {
            return (await AccountsStorage.FindAll()).Where(a => a.Owner.Id.Equals(ownerId)).ToList();
        }
    }
}