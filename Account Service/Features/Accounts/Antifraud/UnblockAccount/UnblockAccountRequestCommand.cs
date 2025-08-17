using MediatR;

namespace Account_Service.Features.Accounts.Antifraud.UnblockAccount
{
    /// <inheritdoc />
    public class UnblockAccountRequestCommand(string message) : IRequest
    {
        /// <summary>
        /// Сообщение события
        /// </summary>
        public string Message { get; set; } = message;
    }
}