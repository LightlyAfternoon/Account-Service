using Account_Service.Features.Accounts;

namespace Account_Service.ObjectStorage
{
    public class AccountsStorage
    {
        private static readonly List<Account> Accounts = new();
        
        public static async Task<Account?> Find(Guid id)
        {
            Account? existedAccount = await Task.Run(() => Accounts.Find(a => a.Id.Equals(id)));

            if (existedAccount != null)
            {
                existedAccount = new Account(existedAccount.Id, existedAccount);
            }

            return existedAccount;
        }

        public static async Task<List<Account>> FindAll()
        {
            return await Task.Run(() => Accounts.Select(account => new Account(account.Id, account)).ToList());
        }

        public static async Task<Account> Add(Account account)
        {
            account = new Account(Guid.NewGuid(), account);

            await Task.Run(() => Accounts.Add(account));

            return account;
        }

        public static async Task<Account?> Update(Account account)
        {
            if (account.Id != Guid.Empty)
            {
                Account? existedAccount = await Task.Run(() => Accounts.Find(a => a.Id.Equals(account.Id)));

                if (existedAccount != null)
                {
                    existedAccount.Owner = account.Owner;
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