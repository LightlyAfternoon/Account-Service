using MediatR;

namespace Account_Service.Features.Accounts.AccrueInterest
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public record AccrueInterestRequestCommand : IRequest<List<AccountDto>>;
}