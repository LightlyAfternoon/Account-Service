using System.Text.Json.Serialization;
using MediatR;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    /// <summary>
    /// Класс для запроса изменения счёта
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ownerId"></param>
    /// <param name="type"></param>
    /// <param name="currency"></param>
    /// <param name="balance"></param>
    /// <param name="interestRate"></param>
    /// <param name="openDate"></param>
    /// <param name="closeDate"></param>
    public class UpdateAccountRequestCommand(Guid id, Guid ownerId, string type, string currency, decimal balance, decimal? interestRate, DateOnly openDate, DateOnly? closeDate) : IRequest<AccountDto>
    {
        /// <summary>
        /// Id счёта
        /// </summary>
        [JsonIgnore] public Guid Id { get; } = id;
        /// <summary>
        /// Id владельца счёта
        /// </summary>
        public Guid OwnerId { get; set; } = ownerId;
        /// <summary>
        /// Тип счёта
        /// </summary>
        public string Type { get; set; } = type;
        /// <summary>
        /// Тип валюты
        /// </summary>
        public string Currency { get; set; } = currency;
        /// <summary>
        /// Баланс
        /// </summary>
        public decimal Balance { get; set; } = balance;
        /// <summary>
        /// Процентная ставка
        /// </summary>
        public decimal? InterestRate { get; set; } = interestRate;
        /// <summary>
        /// Дата открытия
        /// </summary>
        public DateOnly OpenDate { get; set; } = openDate;
        /// <summary>
        /// Дата закрытия
        /// </summary>
        public DateOnly? CloseDate { get; set; } = closeDate;

        /// <inheritdoc />
        public UpdateAccountRequestCommand(AccountDto accountDto) : this(id: accountDto.Id,
            ownerId: accountDto.OwnerId,
            type: accountDto.Type,
            currency: accountDto.Currency,
            balance: accountDto.Balance,
            interestRate: accountDto.InterestRate,
            openDate: accountDto.OpenDate,
            closeDate: accountDto.CloseDate)
        {
        }
    }
}