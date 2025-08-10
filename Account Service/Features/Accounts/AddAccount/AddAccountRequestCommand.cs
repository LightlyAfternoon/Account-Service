using MediatR;

namespace Account_Service.Features.Accounts.AddAccount
{

    /// <summary>
    /// Класс для запроса добавления счёта
    /// </summary>
    /// <param name="ownerId"></param>
    /// <param name="type"></param>
    /// <param name="currency"></param>
    /// <param name="balance"></param>
    /// <param name="interestRate"></param>
    /// <param name="openDate"></param>
    /// <param name="closeDate"></param>
    public class AddAccountRequestCommand(Guid ownerId, string type, string currency, decimal balance, decimal? interestRate, DateOnly openDate, DateOnly? closeDate) : IRequest<AccountDto>
    {
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
    }
}