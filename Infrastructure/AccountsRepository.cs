using Account_Service.Features.Accounts;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    public class AccountsRepository : IAccountsRepository
    {
        public Account? FindById(Guid id)
        {
            return AccountsStorage.Find(id);
        }

        public List<Account> FindAll()
        {
            return AccountsStorage.FindAll();
        }

        public Account? Save(Account entity)
        {
            return AccountsStorage.Update(entity);
        }

        public bool DeleteById(Guid id)
        {
            return AccountsStorage.Delete(id);
        }

        public List<Account> FindAllByOwnerId(Guid ownerId)
        {
            return AccountsStorage.FindAll().Where(a => a.Owner.Id.Equals(ownerId)).ToList();
        }
    }
}