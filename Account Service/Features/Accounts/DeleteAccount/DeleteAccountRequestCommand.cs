using MediatR;

namespace Account_Service.Features.Accounts.DeleteAccount
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record DeleteAccountRequestCommand(Guid Id) : IRequest<bool>;
}