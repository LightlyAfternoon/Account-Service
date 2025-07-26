using MediatR;

namespace Account_Service.Features.Users.GetUser
{
    /// <inheritdoc />
    public class GetUserByIdRequestCommand(Guid id) : IRequest<UserDto>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid Id { get; } = id;
    }
}