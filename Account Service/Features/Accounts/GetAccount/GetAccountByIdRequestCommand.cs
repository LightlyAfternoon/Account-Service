using MediatR;

namespace Account_Service.Features.Accounts.GetAccount
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record GetAccountByIdRequestCommand(Guid Id) : IRequest<AccountDto>;
}