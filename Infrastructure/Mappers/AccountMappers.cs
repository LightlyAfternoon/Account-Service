using Account_Service.Features.Accounts;

namespace Account_Service.Infrastructure.Mappers
{
    /// <inheritdoc />
    public class AccountMappers : IMappers<AccountDto, Account>
    {
        /// <inheritdoc />
        public static AccountDto MapToDto(Account account) => new(id: account.Id,
            ownerId: account.OwnerId,
            type: account.Type.ToString(),
            currency: account.Currency.ToString(),
            balance: account.Balance,
            interestRate: account.InterestRate,
            openDate: account.OpenDate,
            closeDate: account.CloseDate);

        /// <inheritdoc />
        public static Account MapToEntity(AccountDto accountDto) => new(id: accountDto.Id,
            ownerId: accountDto.OwnerId,
            type: Enum.Parse<AccountType>(accountDto.Type),
            currency: Enum.Parse<CurrencyCode>(accountDto.Currency),
            balance: accountDto.Balance,
            interestRate: accountDto.InterestRate,
            openDate: accountDto.OpenDate,
            closeDate: accountDto.CloseDate);
    }
}