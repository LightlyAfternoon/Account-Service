using Account_Service.Features.Accounts;

namespace Account_Service.ObjectStorage
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountsStorage
    {
        private static readonly List<Account> Accounts = new();
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static async Task<Account?> Find(Guid id)
        {
            Account? existedAccount = await Task.Run(() => Accounts.Find(a => a.Id.Equals(id)));

            if (existedAccount != null)
            {
                existedAccount = new Account(existedAccount.Id, existedAccount);

                existedAccount.Transactions = (await TransactionsStorage.FindAll()).Where(t => t.AccountId.Equals(existedAccount.Id)).ToList();
            }

            return existedAccount;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static async Task<List<Account>> FindAll()
        {
            return await Task.Run(() => Accounts.Select(account => new Account(account.Id, account)).ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<Account> Add(Account account)
        {
            account = new Account(Guid.NewGuid(), account);

            await Task.Run(() => Accounts.Add(account));

            return account;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static async Task<Account?> Update(Account account)
        {
            if (account.Id != Guid.Empty)
            {
                Account? existedAccount = await Task.Run(() => Accounts.Find(a => a.Id.Equals(account.Id)));

                if (existedAccount != null)
                {
                    existedAccount.OwnerId = account.OwnerId;
                    existedAccount.Type = account.Type;
                    existedAccount.Currency = account.Currency;
                    existedAccount.Balance = account.Balance;
                    existedAccount.InterestRate = account.InterestRate;
                    existedAccount.OpenDate = account.OpenDate;
                    existedAccount.CloseDate = account.CloseDate;
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
            Account? existedAccount = await Task.Run(() => Accounts.Find(a => a.Id.Equals(id)));

            if (existedAccount != null)
            {
                return await Task.Run(() => Accounts.Remove(existedAccount));
            }

            return false;
        }
    }
}