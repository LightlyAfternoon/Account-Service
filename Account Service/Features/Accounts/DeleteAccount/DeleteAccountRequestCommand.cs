using MediatR;

namespace Account_Service.Features.Accounts.DeleteAccount
{
    /// <inheritdoc />
    public record DeleteAccountRequestCommand(Guid Id) : IRequest<bool>;
}