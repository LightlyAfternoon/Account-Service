using MediatR;

namespace Account_Service.Features.Accounts.GetClientCurrentAccountBalance
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record GetClientCurrentAccountBalanceRequestCommand(Guid OwnerId) : IRequest<GetClientCurrentAccountBalanceResponse>;
}