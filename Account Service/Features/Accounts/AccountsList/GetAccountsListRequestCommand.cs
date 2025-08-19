using MediatR;

namespace Account_Service.Features.Accounts.AccountsList
    // ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record GetAccountsListRequestCommand : IRequest<List<AccountDto>>;
}