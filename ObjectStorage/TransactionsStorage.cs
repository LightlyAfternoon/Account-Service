using Account_Service.Features.Transactions;

namespace Account_Service.ObjectStorage
{
    public class TransactionsStorage
    {
        private static readonly List<Transaction> Transactions = new();

        public static async Task<Transaction?> Find(Guid id)
        {
            Transaction? existedAccount = await Task.Run(() => Transactions.Find(a => a.Id.Equals(id)));

            if (existedAccount != null)
            {
                existedAccount = new Transaction(existedAccount.Id, existedAccount);
            }

            return existedAccount;
        }

        public static async Task<List<Transaction>> FindAll()
        {
            return await Task.Run(() => Transactions.Select(account => new Transaction(account.Id, account)).ToList());
        }

        public static async Task<Transaction> Add(Transaction transaction)
        {
            transaction = new Transaction(Guid.NewGuid(), transaction);

            await Task.Run(() => Transactions.Add(transaction));

            return transaction;
        }

        public static async Task<Transaction?> Update(Transaction transaction)
        {
            if (transaction.Id != Guid.Empty)
            {
                Transaction? existedTransaction = await Task.Run(() => Transactions.Find(a => a.Id.Equals(transaction.Id)));

                if (existedTransaction != null)
                {
                    existedTransaction.Account = transaction.Account;
                    existedTransaction.CounterpartyAccount = transaction.CounterpartyAccount;
                    existedTransaction.Sum = transaction.Sum;
                    existedTransaction.Currency = transaction.Currency;
                    existedTransaction.Type = transaction.Type;
                    existedTransaction.Description = transaction.Description;
                    existedTransaction.DateTime = transaction.DateTime;
                }
            }

            return null;
        }

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