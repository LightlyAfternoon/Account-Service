using System.Text.Json.Serialization;

namespace Account_Service.Features.Accounts
{
    /// <summary>
    /// DTO счёта
    /// </summary>
    /// <param name="id">Id счёта</param>
    /// <param name="ownerId">Id владельца счёта</param>
    /// <param name="type">Тип счёта</param>
    /// <param name="currency">Тип валюты</param>
    /// <param name="balance">Текущий баланс</param>
    /// <param name="interestRate">Процентная ставка (только для типа Deposit и Credit)</param>
    /// <param name="openDate">Дата открытия</param>
    /// <param name="closeDate">Дата закрытия</param>
    [method: JsonConstructor]
    public class AccountDto(
        Guid id,
        Guid ownerId,
        string type,
        string currency,
        decimal balance,
        decimal? interestRate,
        DateOnly openDate,
        DateOnly? closeDate)
    {
        /// <summary>
        /// Id счёта
        /// </summary>
        public Guid Id { get; } = id;
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
        /// Текущий баланс
        /// </summary>
        public decimal Balance { get; set; } = balance;
        /// <summary>
        /// Процентная ставка (только для типа Deposit и Credit)
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
        /// <summary>
        /// Флаг статуса заморозки счёта
        /// </summary>
        public bool Frozen { get; set; }

        /// <inheritdoc />
        public AccountDto(Guid id, AccountDto accountDto) : this(id, accountDto.OwnerId, accountDto.Type, accountDto.Currency, accountDto.Balance, accountDto.InterestRate, accountDto.OpenDate, accountDto.CloseDate)
        {
            Frozen = accountDto.Frozen;
        }

        /// <inheritdoc />
        public override bool Equals(object? obj)
        {
            var account = obj as AccountDto;

            if (account == null)
                return false;
            else
                return Id.Equals(account.Id) && OwnerId.Equals(account.OwnerId) && Type.Equals(account.Type)
                       && Currency.Equals(account.Currency) && Balance.Equals(account.Balance)
                       && InterestRate.Equals(account.InterestRate) && OpenDate.Equals(account.OpenDate)
                       && CloseDate.Equals(account.CloseDate)
                       && Frozen.Equals(account.Frozen);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}