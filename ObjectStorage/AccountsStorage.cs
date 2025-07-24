using Account_Service.Features.Accounts;

namespace Account_Service.ObjectStorage
{
    public class AccountsStorage
    {
        private static readonly List<Account> Accounts = new List<Account>();
        
        public static Account? Find(Guid id)
        {
            Account? existedAccount = Accounts.Find(a => a.Id.Equals(id));

            if (existedAccount != null)
            {
                existedAccount = new Account(existedAccount.Id, existedAccount);
            }

            return existedAccount;
        }

        public static List<Account> FindAll()
        {
            return Accounts.Select(account => new Account(account.Id, account)).ToList();
        }

        public static Account Add(Account account)
        {
            account = new Account(Guid.NewGuid(), account);

            Accounts.Add(account);

            return account;
        }

        public static Account? Update(Account account)
        {
            if (account.Id != Guid.Empty)
            {
                Account? existedAccount = Accounts.Find(a => a.Id.Equals(account.Id));

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

        public static bool Delete(Guid id)
        {
            Account? existedAccount = Accounts.Find(a => a.Id.Equals(id));

            if (existedAccount != null)
            {
                return Accounts.Remove(existedAccount);
            }

            return false;
        }
    }
}