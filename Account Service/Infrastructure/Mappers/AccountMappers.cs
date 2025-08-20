using Account_Service.Features.Accounts;

namespace Account_Service.Infrastructure.Mappers
// ReSharper disable once ArrangeNamespaceBody
{
    /// <summary>
    /// 
    /// </summary>
    public class AccountMappers
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public static AccountDto MapToDto(Account account) => new(id: account.Id,
            ownerId: account.OwnerId,
            type: account.Type.ToString(),
            currency: account.Currency.ToString(),
            balance: account.Balance,
            interestRate: account.InterestRate,
            openDate: account.OpenDate,
            closeDate: account.CloseDate,
            frozen: account.Frozen);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountDto"></param>
        /// <returns></returns>
        public static Account MapToEntity(AccountDto accountDto) => new(id: accountDto.Id,
            ownerId: accountDto.OwnerId,
            type: Enum.Parse<AccountType>(accountDto.Type),
            currency: Enum.Parse<CurrencyCode>(accountDto.Currency),
            balance: accountDto.Balance,
            interestRate: accountDto.InterestRate,
            openDate: accountDto.OpenDate,
            closeDate: accountDto.CloseDate,
            frozen: accountDto.Frozen);
    }
}