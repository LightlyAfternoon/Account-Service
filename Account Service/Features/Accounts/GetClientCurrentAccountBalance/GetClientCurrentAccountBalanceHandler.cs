using Account_Service.Features.Users;
using MediatR;

namespace Account_Service.Features.Accounts.GetClientCurrentAccountBalance
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
            User? user = await _userRepository.FindById(request.OwnerId);

            if (user != null)
            {
                Account? account = (await _accountsRepository.FindAllByOwnerId(user.Id)).OrderBy(a => a.OpenDate)
                    .LastOrDefault(a => a.Type.Equals(AccountType.Checking));

                if (account != null)
                {
                    return new GetClientCurrentAccountBalanceResponse(account.Id, account.OwnerId, account.Balance);
                }
            }

            return null;
        }
    }
}