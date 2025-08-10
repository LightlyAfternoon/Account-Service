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
            AccountDto dto = new AccountDto(id: requestCommand.Id,
                ownerId: requestCommand.OwnerId,
                type: requestCommand.Type,
                currency: requestCommand.Currency,
                balance: requestCommand.Balance,
                interestRate: requestCommand.InterestRate,
                openDate: requestCommand.OpenDate,
                closeDate: requestCommand.CloseDate);

            Account? account = await _accountsRepository.Save(AccountMappers.MapToEntity(dto));

            if (account != null)
            {
                return AccountMappers.MapToDto(account);
            }

            return null;
        }
    }
}