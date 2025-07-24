using Account_Service.Infrastructure;

namespace Account_Service.Features.Accounts
{
    public class AccountsService : IAccountService
    {
        private readonly IAccountsRepository _accountsRepository;

        public AccountsService(IAccountsRepository accountsRepository)
        {
            _accountsRepository = accountsRepository;
        }

        public AccountDto? FindById(Guid id)
        {
            Account? account = _accountsRepository.FindById(id);

            return account != null ? AccountMappers.MapToDto(account) : null;
        }

        public List<AccountDto> FindAll()
        {
            return _accountsRepository.FindAll().Select(AccountMappers.MapToDto).ToList();
        }

        public AccountDto? Add(AccountDto dto)
        {
            dto = new AccountDto(Guid.Empty, dto);

            Account? account = _accountsRepository.Save(AccountMappers.MapToEntity(dto));

            if (account != null)
            {
                return AccountMappers.MapToDto(account);
            }

            return null;
        }

        public AccountDto? Update(Guid id, AccountDto dto)
        {
            dto = new AccountDto(id, dto);

            Account? account = _accountsRepository.Save(AccountMappers.MapToEntity(dto));

            if (account != null)
            {
                return AccountMappers.MapToDto(account);
            }

            return null;
        }

        public bool DeleteById(Guid id)
        {
            return _accountsRepository.DeleteById(id);
        }

        public bool ClientWithIdHasAnyAccount(Guid id)
        {
            return _accountsRepository.FindAllByOwnerId(id).Count > 0;
        }
    }
}