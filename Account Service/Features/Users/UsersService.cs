using Account_Service.Features.Users.GetUser;
using MediatR;

namespace Account_Service.Features.Users
// ReSharper disable once ArrangeNamespaceBody
{
    /// <inheritdoc />
    public class UsersService : IUsersService
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mediator"></param>
        public UsersService(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <inheritdoc />
        public async Task<UserDto?> FindById(Guid id)
        {
            return await _mediator.Send(new GetUserByIdRequestCommand(id));
        }
    }
}