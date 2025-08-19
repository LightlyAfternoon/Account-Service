using Account_Service.Features.Accounts.AccountsList;
using Account_Service.Features.Accounts.AccrueInterest;
using Account_Service.Features.Accounts.AddAccount;
using Account_Service.Features.Accounts.Antifraud.BlockAccount;
using Account_Service.Features.Accounts.Antifraud.UnblockAccount;
using Account_Service.Features.Accounts.DeleteAccount;
using Account_Service.Features.Accounts.GetAccount;
using Account_Service.Features.Accounts.GetClientCurrentAccountBalance;
using Account_Service.Features.Accounts.UpdateAccount;
using Account_Service.Features.Accounts.UserAccount;
using MediatR;

namespace Account_Service.Features.Accounts
    // ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class AccountsService : IAccountsService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public AccountsService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<AccountDto?> FindById(Guid id)
        {
            return await _mediator.Send(new GetAccountByIdRequestCommand(id));
        }

        /// <inheritdoc />
        public async Task<List<AccountDto>> FindAll()
        {
            return await _mediator.Send(new GetAccountsListRequestCommand());
        }

        /// <inheritdoc />
        public async Task<AccountDto?> Add(AddAccountRequestCommand requestCommand)
        {
            var addAccountRequestCommand = new AddAccountRequestCommand(ownerId: requestCommand.OwnerId,
                type: requestCommand.Type,
                currency: requestCommand.Currency,
                balance: requestCommand.Balance,
                interestRate: requestCommand.InterestRate,
                openDate: requestCommand.OpenDate,
                closeDate: requestCommand.CloseDate);

            return await _mediator.Send(addAccountRequestCommand);
        }

        /// <inheritdoc />
        public async Task<AccountDto?> Update(Guid id, UpdateAccountRequestCommand requestCommand)
        {
            var updateAccountRequestCommand = new UpdateAccountRequestCommand(id: id,
                ownerId: requestCommand.OwnerId,
                type: requestCommand.Type,
                currency: requestCommand.Currency,
                balance: requestCommand.Balance,
                interestRate: requestCommand.InterestRate,
                openDate: requestCommand.OpenDate,
                closeDate: requestCommand.CloseDate);

            return await _mediator.Send(updateAccountRequestCommand);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteById(Guid id)
        {
            return await _mediator.Send(new DeleteAccountRequestCommand(id));
        }

        /// <inheritdoc />
        public async Task<bool> ClientWithIdHasAnyAccount(Guid ownerId)
        {
            return await _mediator.Send(new ClientWithIdHasAnyAccountRequestCommand(ownerId));
        }

        /// <inheritdoc />
        public async Task<GetClientCurrentAccountBalanceResponse?> GetClientCurrentAccountBalance(Guid ownerId)
        {
            return await _mediator.Send(new GetClientCurrentAccountBalanceRequestCommand(ownerId));
        }

        /// <inheritdoc />
        public async Task ProcessDailyAccrueInterest()
        {
            await _mediator.Send(new AccrueInterestRequestCommand());
        }

        /// <inheritdoc />
        public async Task BlockAccount(string message)
        {
            await _mediator.Send(new BlockAccountRequestCommand(message));
        }

        /// <inheritdoc />
        public async Task UnblockAccount(string message)
        {
            await _mediator.Send(new UnblockAccountRequestCommand(message));
        }
    }
}