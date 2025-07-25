using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Accounts.AccountsList
{
    public class GetAccountsListHandler : IRequestHandler<GetAccountsListRequestCommand, List<AccountDto>>
    {
        private readonly IAccountsRepository _accountsRepository;

        public GetAccountsListHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public async Task<List<AccountDto>> Handle(GetAccountsListRequestCommand requestCommand, CancellationToken cancellationToken)
        {
            return (await _accountsRepository.FindAll()).Select(AccountMappers.MapToDto).ToList();
        }
    }
}