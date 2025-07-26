using Account_Service.Features.Transactions;
using Account_Service.ObjectStorage;

namespace Account_Service.Infrastructure
{
    /// <inheritdoc />
    public class TransactionsRepository : ITransactionsRepository
    {
        /// <inheritdoc />
        public async Task<Transaction?> FindById(Guid id)
        {
            return await TransactionsStorage.Find(id);
        }

        /// <inheritdoc />
        public async Task<List<Transaction>> FindAll()
        {
            return await TransactionsStorage.FindAll();
        }

        /// <inheritdoc />
        public async Task<Transaction?> Save(Transaction entity)
        {
            if (entity.Id == Guid.Empty)
            {
                return await TransactionsStorage.Add(entity);
            }
            else
            {
                return await TransactionsStorage.Update(entity);
            }
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            return await TransactionsStorage.Delete(id);
        }
    }
}