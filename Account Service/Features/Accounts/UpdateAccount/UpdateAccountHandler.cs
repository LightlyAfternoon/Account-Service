using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    /// <inheritdoc />
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountRequestCommand, AccountDto?>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public UpdateAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<AccountDto?> Handle(UpdateAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            Account? account = await _accountsRepository.FindById(requestCommand.Id);

            if (account != null)
            {
                account.OwnerId = requestCommand.OwnerId;
                account.Type = Enum.Parse<AccountType>(requestCommand.Type);
                account.Currency = Enum.Parse<CurrencyCode>(requestCommand.Currency);
                account.Balance = requestCommand.Balance;
                account.InterestRate = requestCommand.InterestRate;
                account.OpenDate = requestCommand.OpenDate;
                account.CloseDate = requestCommand.CloseDate;

                account = await _accountsRepository.Save(account, cancellationToken);

                return AccountMappers.MapToDto(account!);
            }

            return null;
        }
    }
}