using Account_Service.Infrastructure;
using MediatR;

namespace Account_Service.Features.Accounts.GetAccount
{
    /// <inheritdoc />
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdRequestCommand, AccountDto?>
    {
        private readonly IAccountsRepository _accountsRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="accountsRepository"></param>
        public GetAccountByIdHandler(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        /// <inheritdoc />
        public async Task<AccountDto?> Handle(GetAccountByIdRequestCommand request, CancellationToken cancellationToken)
        {
            Account? account = await _accountsRepository.FindById(request.Id);

            if (account != null)
            {
                return AccountMappers.MapToDto(account);
            }

            return null;
        }
    }
}