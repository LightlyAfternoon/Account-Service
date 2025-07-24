using Account_Service.Features.Transactions;

namespace Account_Service.ObjectStorage
{
    public class TransactionsStorage
    {
        private static readonly List<Transaction> Transactions = new List<Transaction>();

        public static Transaction? Find(Guid id)
        {
            Transaction? existedAccount = Transactions.Find(a => a.Id.Equals(id));

            if (existedAccount != null)
            {
                existedAccount = new Transaction(existedAccount.Id, existedAccount);
            }

            return existedAccount;
        }

        public static List<Transaction> FindAll()
        {
            return Transactions.Select(account => new Transaction(account.Id, account)).ToList();
        }

        public static Transaction Add(Transaction transaction)
        {
            transaction = new Transaction(Guid.NewGuid(), transaction);

            Transactions.Add(transaction);

            return transaction;
        }

        public static Transaction? Update(Transaction transaction)
        {
            if (transaction.Id != Guid.Empty)
            {
                Transaction? existedTransaction = Transactions.Find(a => a.Id.Equals(transaction.Id));

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

        public static bool Delete(Guid id)
        {
            Transaction? existedTransaction = Transactions.Find(a => a.Id.Equals(id));

            if (existedTransaction != null)
            {
                return Transactions.Remove(existedTransaction);
            }

            return false;
        }
    }
}