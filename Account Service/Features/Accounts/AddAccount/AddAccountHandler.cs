using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Accounts.AddAccount
{
    /// <inheritdoc />
    public class AddAccountHandler : IRequestHandler<AddAccountRequestCommand, AccountDto?>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public AddAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<AccountDto?> Handle(AddAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            AccountDto dto = new AccountDto(id: Guid.Empty,
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