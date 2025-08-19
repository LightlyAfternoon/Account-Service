using Account_Service.Infrastructure.Mappers;
using MediatR;

namespace Account_Service.Features.Accounts.GetAccount
// ReSharper disable once ArrangeNamespaceBody
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
            var account = await _accountsRepository.FindById(request.Id);

            return account == null ? null : AccountMappers.MapToDto(account);
        }
    }
}