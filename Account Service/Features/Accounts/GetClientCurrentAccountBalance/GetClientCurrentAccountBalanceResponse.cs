using MediatR;

namespace Account_Service.Features.Accounts.GetClientCurrentAccountBalance
{
    /// <summary>
    /// Класс для запроса получения баланса текущего счёта клиента
    /// </summary>
    /// <param name="AccountId">Id счёта</param>
    /// <param name="OwnerId">Id клиента</param>
    /// <param name="Balance">Баланс</param>
    public record GetClientCurrentAccountBalanceResponse(Guid AccountId, Guid OwnerId, decimal Balance) : IRequest<AccountDto>;
}