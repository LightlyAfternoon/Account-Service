using MediatR;

namespace Account_Service.Features.Users.GetUser
// ReSharper disable once ArrangeNamespaceBody
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