using Account_Service.Features.Transactions;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    public class TransactionsRepository : ITransactionsRepository
    {
        public Transaction? FindById(Guid id)
        {
            return TransactionsStorage.Find(id);
        }

        public List<Transaction> FindAll()
        {
            return TransactionsStorage.FindAll();
        }

        public Transaction? Save(Transaction entity)
        {
            return TransactionsStorage.Update(entity);
        }

        public bool DeleteById(Guid id)
        {
            return TransactionsStorage.Delete(id);
        }
    }
}