using Account_Service.Features.Accounts.AccountsList;
using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.DeleteAccount;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Features.Accounts.UserAccount;
using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Accounts
{
    public class AccountsService : IAccountService
    {
        private readonly IMediator _mediator;

        public AccountsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<List<AccountDto>> FindAll()
        {
            return await _mediator.Send(new GetAccountsListRequestCommand());
        }

        public async Task<AccountDto?> Add(AccountDto dto)
        {
            var addAccountRequestCommand = new AddAccountRequestCommand()
            {
                Owner = dto.Owner,
                Type = dto.Type,
                Currency = dto.Currency,
                Balance = dto.Balance,
                InterestRate = dto.InterestRate,
                OpenDate = dto.OpenDate,
                CloseDate = dto.CloseDate
            };

            return await _mediator.Send(addAccountRequestCommand);
        }

        public async Task<AccountDto?> Update(Guid id, AccountDto dto)
        {
            var updateAccountRequestCommand = new UpdateAccountRequestCommand(id)
            {
                Owner = dto.Owner,
                Type = dto.Type,
                Currency = dto.Currency,
                Balance = dto.Balance,
                InterestRate = dto.InterestRate,
                OpenDate = dto.OpenDate,
                CloseDate = dto.CloseDate
            };

            return await _mediator.Send(updateAccountRequestCommand);
        }

        public async Task<bool> DeleteById(Guid id)
        {
            return await _mediator.Send(new DeleteAccountRequestCommand(id));
        }

        public async Task<bool> ClientWithIdHasAnyAccount(Guid ownerId)
        {
            return await _mediator.Send(new ClientWithIdHasAnyAccountRequestCommand(ownerId));
        }
    }
}