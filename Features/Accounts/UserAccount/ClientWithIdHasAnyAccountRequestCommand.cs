using MediatR;

namespace Account_Service.Features.Accounts.UserAccount
{
    /// <inheritdoc />
    public record ClientWithIdHasAnyAccountRequestCommand(Guid OwnerId) : IRequest<bool>;
}