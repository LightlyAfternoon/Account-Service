using Account_Service.Infrastructure;

namespace Account_Service.Features.Accounts
{
    public interface IAccountService : IService<AccountDto>
    {
        bool ClientWithIdHasAnyAccount(Guid id);
    }
}