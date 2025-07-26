using Account_Service.Features.Transactions;

namespace Account_Service.ObjectStorage
{
    /// <summary>
    /// 
    /// </summary>
    public class TransactionsStorage
    {
        private static readonly List<Transaction> Transactions = new();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Transaction?> Find(Guid id)
        {
            Transaction? existedAccount = await Task.Run(() => Transactions.Find(a => a.Id.Equals(id)));

            if (existedAccount != null)
            {
                existedAccount = new Transaction(existedAccount.Id, existedAccount);
            }

            return existedAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Transaction>> FindAll()
        {
            return await Task.Run(() => Transactions.Select(account => new Transaction(account.Id, account)).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<Transaction> Add(Transaction transaction)
        {
            transaction = new Transaction(Guid.NewGuid(), transaction);

            await Task.Run(() => Transactions.Add(transaction));

            return transaction;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public static async Task<Transaction?> Update(Transaction transaction)
        {
            if (transaction.Id != Guid.Empty)
            {
                Transaction? existedTransaction = await Task.Run(() => Transactions.Find(a => a.Id.Equals(transaction.Id)));

                if (existedTransaction != null)
                {
                    existedTransaction.AccountId = transaction.AccountId;
                    existedTransaction.CounterpartyAccountId = transaction.CounterpartyAccountId;
                    existedTransaction.Sum = transaction.Sum;
                    existedTransaction.Currency = transaction.Currency;
                    existedTransaction.Type = transaction.Type;
                    existedTransaction.Description = transaction.Description;
                    existedTransaction.DateTime = transaction.DateTime;
                }
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<bool> Delete(Guid id)
        {
            Transaction? existedTransaction = await Task.Run(() => Transactions.Find(a => a.Id.Equals(id)));

            if (existedTransaction != null)
            {
                return await Task.Run(() => Transactions.Remove(existedTransaction));
            }

            return false;
        }
    }
}