using MediatR;

namespace Account_Service.Features.Accounts.UserAccount
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record ClientWithIdHasAnyAccountRequestCommand(Guid OwnerId) : IRequest<bool>;
}