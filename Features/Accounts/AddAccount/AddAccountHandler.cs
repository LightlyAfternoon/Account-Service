using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Accounts.AddAccount
{
    public class AddAccountHandler : IRequestHandler<AddAccountRequestCommand, AccountDto?>
    {
        private readonly IAccountsRepository _accountsRepository;

        public AddAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<AccountDto?> Handle(AddAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            AccountDto dto = new AccountDto(Guid.Empty)
            {
                Owner = requestCommand.Owner,
                Type = requestCommand.Type,
                Currency = requestCommand.Currency,
                Balance = requestCommand.Balance, InterestRate = requestCommand.InterestRate,
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