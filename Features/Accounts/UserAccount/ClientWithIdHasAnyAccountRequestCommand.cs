using MediatR;

namespace Account_Service.Features.Accounts.UserAccount
{
    public record ClientWithIdHasAnyAccountRequestCommand(Guid OwnerId) : IRequest<bool>;
}