using Account_Service.Features.Transactions;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    public class TransactionsRepository : ITransactionsRepository
    {
        public async Task<Transaction?> FindById(Guid id)
        {
            return await TransactionsStorage.Find(id);
        }

        public async Task<List<Transaction>> FindAll()
        {
            return await TransactionsStorage.FindAll();
        }

        public async Task<Transaction?> Save(Transaction entity)
        {
            return await TransactionsStorage.Update(entity);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            return await TransactionsStorage.Delete(id);
        }
    }
}