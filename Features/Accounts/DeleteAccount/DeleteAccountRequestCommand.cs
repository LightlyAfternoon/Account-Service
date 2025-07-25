using MediatR;

namespace Account_Service.Features.Accounts.DeleteAccount
{
    public record DeleteAccountRequestCommand(Guid Id) : IRequest<bool>;
}