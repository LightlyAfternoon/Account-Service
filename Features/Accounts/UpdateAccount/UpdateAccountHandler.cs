using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    public class UpdateAccountHandler : IRequestHandler<UpdateAccountRequestCommand, AccountDto?>
    {
        private readonly IAccountsRepository _accountsRepository;

        public UpdateAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<AccountDto?> Handle(UpdateAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            AccountDto dto = new AccountDto(requestCommand.Id)
            {
                Owner = requestCommand.Owner,
                Type = requestCommand.Type,
                Currency = requestCommand.Currency,
                Balance = requestCommand.Balance,
                InterestRate = requestCommand.InterestRate,
                OpenDate = requestCommand.OpenDate,
                CloseDate = requestCommand.CloseDate
            };

            Account? account = await _accountsRepository.Save(AccountMappers.MapToEntity(dto));

            if (account != null)
            {
                return AccountMappers.MapToDto(account);
            }

            return null;
        }
    }
}