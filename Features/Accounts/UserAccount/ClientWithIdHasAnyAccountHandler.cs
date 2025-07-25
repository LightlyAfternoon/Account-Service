using MediatR;

namespace Account_Service.Features.Accounts.UserAccount
{
    public class ClientWithIdHasAnyAccountHandler : IRequestHandler<ClientWithIdHasAnyAccountRequestCommand, bool>
    {
        private readonly IAccountsRepository _accountsRepository;

        public ClientWithIdHasAnyAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<bool> Handle(ClientWithIdHasAnyAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _accountsRepository.FindAllByOwnerId(requestCommand.OwnerId)).Count > 0;
        }
    }
}