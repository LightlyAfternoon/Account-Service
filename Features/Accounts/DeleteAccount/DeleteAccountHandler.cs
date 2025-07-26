using MediatR;

namespace Account_Service.Features.Accounts.DeleteAccount
{
    /// <inheritdoc />
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountRequestCommand, bool>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public DeleteAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<bool> Handle(DeleteAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return await _accountsRepository.DeleteById(requestCommand.Id);
        }
    }
}