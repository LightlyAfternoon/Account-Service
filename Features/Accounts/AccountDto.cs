using System.Text.Json.Serialization;

namespace Account_Service.Features.Accounts
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="ownerId"></param>
    /// <param name="type"></param>
    /// <param name="currency"></param>
    /// <param name="balance"></param>
    /// <param name="interestRate"></param>
    /// <param name="openDate"></param>
    /// <param name="closeDate"></param>
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
        /// 
        /// </summary>
        public Guid Id { get; } = id;
        /// <summary>
        /// 
        /// </summary>
        public Guid OwnerId { get; set; } = ownerId;
        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; } = type;
        /// <summary>
        /// 
        /// </summary>
        public string Currency { get; set; } = currency;
        /// <summary>
        /// 
        /// </summary>
        public decimal Balance { get; set; } = balance;
        /// <summary>
        /// 
        /// </summary>
        public decimal? InterestRate { get; set; } = interestRate;
        /// <summary>
        /// 
        /// </summary>
        public DateOnly OpenDate { get; set; } = openDate;
        /// <summary>
        /// 
        /// </summary>
        public DateOnly? CloseDate { get; set; } = closeDate;

        /// <inheritdoc />
        public AccountDto(Guid id, AccountDto accountDto) : this(id, accountDto.OwnerId, accountDto.Type, accountDto.Currency, accountDto.Balance, accountDto.InterestRate, accountDto.OpenDate, accountDto.CloseDate)
        {
        }
    }
}