using System.Text.Json.Serialization;
using MediatR;

namespace Account_Service.Features.Accounts.UpdateAccount
{
    /// <inheritdoc />
    public class UpdateAccountRequestCommand(Guid id, Guid ownerId, string type, string currency, decimal balance, decimal? interestRate, DateOnly openDate, DateOnly? closeDate) : IRequest<AccountDto>
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore] public Guid Id { get; } = id;
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