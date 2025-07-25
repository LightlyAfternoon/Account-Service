using MediatR;

namespace Account_Service.Features.Accounts.AccountsList
{
    public record GetAccountsListRequestCommand : IRequest<List<AccountDto>>;
}