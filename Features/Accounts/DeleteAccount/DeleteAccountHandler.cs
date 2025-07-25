using MediatR;

namespace Account_Service.Features.Accounts.DeleteAccount
{
    public class DeleteAccountHandler : IRequestHandler<DeleteAccountRequestCommand, bool>
    {
        private readonly IAccountsRepository _accountsRepository;

        public DeleteAccountHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<bool> Handle(DeleteAccountRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return await _accountsRepository.DeleteById(requestCommand.Id);
        }
    }
}