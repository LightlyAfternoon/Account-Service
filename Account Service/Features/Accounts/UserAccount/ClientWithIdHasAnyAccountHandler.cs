using MediatR;

namespace Account_Service.Features.Accounts.UserAccount
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class ClientWithIdHasAnyAccountHandler : IRequestHandler<ClientWithIdHasAnyAccountRequestCommand, bool>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public ClientWithIdHasAnyAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(ClientWithIdHasAnyAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _accountsRepository.FindAllByOwnerId(requestCommand.OwnerId)).Count > 0;
        }
    }
}