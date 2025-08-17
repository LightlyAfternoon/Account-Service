using MediatR;

namespace Account_Service.Features.Accounts.Antifraud.BlockAccount
{
    /// <inheritdoc />
    public class BlockAccountRequestCommand(string message) : IRequest
    {
        /// <summary>
        /// Сообщение события
        /// </summary>
        public string Message { get; set; } = message;
    }
}