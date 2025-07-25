using Account_Service.Infrastructure;

namespace Account_Service.Features.Accounts
{
    public interface IAccountService : IService<AccountDto>
    {
        Task<bool> ClientWithIdHasAnyAccount(Guid id);
    }
}