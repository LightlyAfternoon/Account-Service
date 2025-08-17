using MediatR;

namespace Account_Service.Features.Accounts.AccrueInterest
{
    /// <inheritdoc />
    public record AccrueInterestRequestCommand : IRequest<List<AccountDto>>;
}