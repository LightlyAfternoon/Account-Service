using MediatR;

namespace Account_Service.Features.Accounts.GetClientCurrentAccountBalance
{
    /// <inheritdoc />
    public record GetClientCurrentAccountBalanceRequestCommand(Guid OwnerId) : IRequest<GetClientCurrentAccountBalanceResponse>;
}