using MediatR;

namespace Account_Service.Features.Accounts.AccountsList
{
    /// <inheritdoc />
    public record GetAccountsListRequestCommand : IRequest<List<AccountDto>>;
}