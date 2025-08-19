using Account_Service.Features.Users;
using MediatR;

namespace Account_Service.Features.Accounts.GetClientCurrentAccountBalance
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class GetClientCurrentAccountBalanceHandler : IRequestHandler<GetClientCurrentAccountBalanceRequestCommand, GetClientCurrentAccountBalanceResponse?>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userRepository"></param>
        /// <param name="accountsRepository"></param>
        public GetClientCurrentAccountBalanceHandler(IUserRepository userRepository, IAccountsRepository accountsRepository)
        {
            _userRepository = userRepository;
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<GetClientCurrentAccountBalanceResponse?> Handle(GetClientCurrentAccountBalanceRequestCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.FindById(request.OwnerId);

            if (user == null)
                return null;

            var account = (await _accountsRepository.FindAllByOwnerId(user.Id)).OrderBy(a => a.OpenDate)
                .LastOrDefault(a => a.Type.Equals(AccountType.Checking));

            return account != null ? new GetClientCurrentAccountBalanceResponse(account.Id, account.OwnerId, account.Balance) : null;
        }
    }
}