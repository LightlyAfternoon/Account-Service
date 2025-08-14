using MediatR;

namespace Account_Service.Features.Accounts.GetAccount
{
    /// <inheritdoc />
    public record GetAccountByIdRequestCommand(Guid Id) : IRequest<AccountDto>;
}