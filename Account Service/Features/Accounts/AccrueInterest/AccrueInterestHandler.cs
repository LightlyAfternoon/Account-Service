using MediatR;

namespace Account_Service.Features.Accounts.AccrueInterest
{
    /// <inheritdoc />
    public class AccrueInterestHandler : IRequestHandler<AccrueInterestRequestCommand>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public AccrueInterestHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task Handle(AccrueInterestRequestCommand request, CancellationToken cancellationToken)
        {
            await _accountsRepository.AccrueInterestForAllOpenedAccounts();
        }
    }
}