using Account_Service.Features.Accounts;

namespace Account_Service.Infrastructure
{
    public class AccountMappers
    {
        public static AccountDto MapToDto(Account account) => new(account.Id)
        {
            Owner = new User(account.Owner.Id, account.Owner),
            Type = account.Type,
            Currency = account.Currency,
            Balance = account.Balance,
            InterestRate = account.InterestRate,
            OpenDate = account.OpenDate,
            CloseDate = account.CloseDate
        };

        public static Account MapToEntity(AccountDto accountDto) => new(accountDto.Id)
        {
            Owner = new User(accountDto.Owner.Id, accountDto.Owner),
            Type = accountDto.Type,
            Currency = accountDto.Currency,
            Balance = accountDto.Balance,
            InterestRate = accountDto.InterestRate,
            OpenDate = accountDto.OpenDate,
            CloseDate = accountDto.CloseDate
        };
    }
}